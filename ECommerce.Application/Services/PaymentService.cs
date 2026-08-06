using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Baskets;
using ECommerce.Application.Specifications;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    internal class PaymentService : IPaymentService
    {
        private readonly IPaymentGateway _stripePaymentGateway;
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PaymentGatewaySecrets _paymentSettings;

        public PaymentService(
            IPaymentGateway stripePaymentGateway,
            IBasketRepository basketRepo,
            IUnitOfWork unitOfWork,
            IOptions<PaymentGatewaySecrets> paymentSettings,
            IMapper mapper)
        {
            _stripePaymentGateway = stripePaymentGateway;
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentSettings = paymentSettings.Value;
        }

        //Create Or Update -> We need to connect to Stripe
        //We created StripeGateway class that connects to stripe, using DI we can use it here
        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default)
        {
            #region Get Basket And Validate
            var basket = await _basketRepo.GetBasketAsync(basketId, ct);
            if (basket == null)
                return Result<BasketDto>.Fail(Error.NotFound("Basket Not Found", $"Could Not Find A Basket With Id {basketId}"));

            if (basket.Items.Count <= 0)
                return Result<BasketDto>.Fail(Error.Validation("Basket Is Empty", $"Basket With Id {basketId} Is Empty, Cannot Create Payment Intent With Empty Basket!"));

            #endregion

            #region Get Delivery Method Cost
            if (!basket.DeliveryMethodId.HasValue)
                return Result<BasketDto>.Fail(Error.Validation("Delivery Method Id Is Required", $"Basket With Id {basketId} Has No Delivery Method Id!"));

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value, ct);

            if (deliveryMethod is null)
                return Result<BasketDto>.Fail(Error.NotFound("Delivery Method Not Found", $"Could Not Find A Delivery Method With Id {basket.DeliveryMethodId.Value}!"));

            basket.ShippingPrice = deliveryMethod.Cost;
            #endregion

            #region Validate Product Prices
            var spec = new ProductWithIdSpecifications((basket.Items.Select(x => x.Id)).ToHashSet());
            var products = (await _unitOfWork.GetRepository<Product, int>().GetAllAsync(spec, ct)).ToDictionary(x => x.Id); ;

            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Result<BasketDto>.Fail(Error.Validation("Item Not Found!", $"Basket Item With Id {item.Id} Is Not An Existing Product!"));
                item.Price = product.Price;
            }
            #endregion

            #region Calculate The Total Amount (Delivery Method Cost + SubTotal Of Item Prcies)

            var subtotal = basket.Items.Sum(i => i.Price * i.Quantity);

            //We will store the amount in the smallest currency unit (in our case it's the 'Cent').
            //To convert from 'Dollars' to 'Cents' we simply multiply by 100 (m for decimal)
            //In the PaymentGateway methods it will be converted from 'Decimal' to 'Long'
            var amount = (subtotal + deliveryMethod.Cost) * 100m;

            #endregion

            #region PaymentIntentId Is Empty ? Create : Update

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var paymentIntentRes = await _stripePaymentGateway.CreatePaymentIntentAsync(amount, _paymentSettings.DefaultCurrency);
                basket.PaymentIntentId = paymentIntentRes.PaymentIntentId;
                basket.ClientSecret = paymentIntentRes.ClientSecret;
            }
            else
            {
                await _stripePaymentGateway.UpdatePaymentIntentAsync(amount, basket.PaymentIntentId, ct);
            }
            #endregion

            #region Update Basket And Return It
            await _basketRepo.CreateOrUpdateBasketAsync(basket, ct: ct);

            return _mapper.Map<BasketDto>(basket);
            #endregion
        }

        public async Task PaymentFailedAsync(string paymentIntentId, CancellationToken ct = default)
        {
            var spec = new OrderWithPaymentIntentSpecifications(paymentIntentId);
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(spec, ct);
            if (order == null)
                return;

            order.Status = OrderStatus.PaymentFailed;
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task PaymentSucceededAsync(string paymentIntentId, CancellationToken ct = default)
        {
            var spec = new OrderWithPaymentIntentSpecifications(paymentIntentId);
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(spec, ct);
            if (order == null)
                return;

            order.Status = OrderStatus.PaymentRecieved;
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
