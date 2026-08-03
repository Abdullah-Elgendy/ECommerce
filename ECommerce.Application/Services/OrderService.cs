using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Specifications;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default)
        {
            //Get basket
            var basket = await _basketService.GetBasketAsync(orderDto.BasketId, ct);
            if (!basket.IsSuccess) return Result<OrderToReturnDto>.Fail(Error.NotFound("Basket Not Found", $"Basket With Id {orderDto.BasketId} Is Not Found!"));

            if (!basket.Value.Items.Any() || basket.Value.Items.Count <= 0)
                return Result<OrderToReturnDto>.Fail(Error.Validation("Basket Is Empty", $"Cannot Create Order, Basket With Id {orderDto.BasketId} Is Empty!"));

            //Get Items from basket
            var orderItems = new List<OrderItem>(basket.Value.Items.Count);

            var productIds = basket.Value.Items.Select(x => x.Id).ToHashSet();
            var spec = new ProductWithIdSpecifications(productIds);
            var verifiedBasketProducts = (await _unitOfWork.GetRepository<Product, int>().GetAllAsync(spec, ct)).ToDictionary(x => x.Id);

            foreach (var item in basket.Value.Items)
            {
                if (!verifiedBasketProducts.TryGetValue(item.Id, out var basketProduct))
                    return Error.NotFound("Product Not Found", $"Product With Id {item.Id} Is Not Found!");
                else
                {
                    orderItems.Add(new OrderItem()
                    {
                        Price = basketProduct.Price,
                        Quantity = item.Quantity,
                        Product = new OrderedProduct()
                        {
                            ProductId = basketProduct.Id,
                            PictureUrl = basketProduct.PictureUrl,
                            ProductName = basketProduct.Name
                        }
                    });
                }
            }

            //Get shipping address
            var orderAddress = _mapper.Map<OrderAddress>(orderDto.ShippingAddress);


            //Get Delivery method
            //we need to get the delivery method to get the 'cost' in order to calculate the total.
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId);
            if (deliveryMethod is null)
                return Error.NotFound("Delivery Method Not Found", $"Delivery Method With Id {orderDto.DeliveryMethodId} Is Not Found!");

            //Calculate Sub Total
            decimal subtotal = orderItems.Sum(x => x.Price * x.Quantity);

            //We DON'T need to calculate the total because it's automatically calculated.

            //Create Order
            var order = new Order(email, orderAddress, orderItems, deliveryMethod, subtotal);

            _unitOfWork.GetRepository<Order, Guid>().Add(order);
            var res = await _unitOfWork.SaveChangesAsync(ct);

            if (res > 0)
            {
                await _basketService.DeleteBasketAsync(orderDto.BasketId, ct);
                return Result<OrderToReturnDto>.Ok(_mapper.Map<OrderToReturnDto>(order));
            }

            else
            {
                return Result<OrderToReturnDto>.Fail(Error.Failure("Order Save Failed", "Could Not Create Order!"));
            }


        }
        public async Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForUserAsync(string email, CancellationToken ct = default)
        {
            var spec = new OrdersSpecifications(email);
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec, ct);

            if (orders.Any())
            {
                var mappedOrders = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
                return Result<IReadOnlyList<OrderToReturnDto>>.Ok(mappedOrders);
            }
            else
            {
                return Result<IReadOnlyList<OrderToReturnDto>>.Fail(Error.NotFound("Orders Not Found", $"User With Email {email} Has No Orders!"));
            }
        }
        public async Task<Result<OrderToReturnDto>> GetOrderForUserByIdAndEmailAsync(Guid id, string email, CancellationToken ct = default)
        {
            var spec = new OrdersSpecifications(id, email);
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(spec, ct);

            if (order is null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound($"Order Not Found", $"Order With Id {id} Not Found For User With Email {email}!"));
            else
            {
                var mappedOrder = _mapper.Map<OrderToReturnDto>(order);
                return Result<OrderToReturnDto>.Ok(mappedOrder);
            }
        }
        public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync(CancellationToken ct = default)
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync(ct);
            if (deliveryMethods.Any())
            {
                var mappedDeliveryMethods = _mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryMethods);
                return Result<IReadOnlyList<DeliveryMethodDto>>.Ok(mappedDeliveryMethods);
            }
            else
                return Result<IReadOnlyList<DeliveryMethodDto>>.Fail(Error.NotFound("Delivery Methods Not Found", "Failed To Retrieve Delivery Methods!"));

        }

    }
}
