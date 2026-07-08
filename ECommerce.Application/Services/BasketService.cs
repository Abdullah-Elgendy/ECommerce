using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Baskets;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    internal class BasketService(IBasketRepository basketRepo, IMapper mapper) : IBasketService
    {
        public async Task<Result<BasketDto>> GetBasketAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await basketRepo.GetBasketAsync(basketId, ct);
            if (basket is null) return Result<BasketDto>.Fail(Error.NotFound("BasketGet.NotFound", $"Basket With Id {basketId} Not Found"));

            var res = mapper.Map<BasketDto>(basket);
            return Result<BasketDto>.Ok(res);
        }
        public async Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto dto, TimeSpan? timeToLive = default, CancellationToken ct = default)
        {
            var basket = mapper.Map<CustomerBasket>(dto);
            var res = await basketRepo.CreateOrUpdateBasketAsync(basket, timeToLive ,ct);

            if (res is null) return Result<BasketDto>.Fail(Error.Failure("BasketCreate.Failure", "Cannot Create Or Update Basket"));
            return Result<BasketDto>.Ok(dto);
        }

        public async Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await basketRepo.GetBasketAsync(basketId, ct);
            if (basket is null)
                return Result<bool>.Fail(Error.NotFound("BasketDelete.NotFound", $"Cannot Delete Basket With Id {basketId}, Not Found"));

            var result = await basketRepo.DeleteBasket(basketId, ct);

            return result ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("BasketDelete.Failure", $"Can Not Delete Basket With Id {basketId}"));
        }

    }
}
