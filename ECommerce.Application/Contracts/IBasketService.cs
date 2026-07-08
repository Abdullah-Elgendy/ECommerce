using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IBasketService
    {
        //Get by id -> take basket id -> return basketDto
        Task<Result<BasketDto>> GetBasketAsync(string basketId, CancellationToken ct = default);

        //Create or Update basket -> Take Basket -> Return basketDto after creation or update
        Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket, TimeSpan? timeToLive = default, CancellationToken ct = default);

        //Delete basket -> Take Basket Id -> return bool
        Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default);

    }
}
