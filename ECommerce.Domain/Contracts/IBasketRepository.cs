using ECommerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface IBasketRepository
    {
        //Get
        Task<CustomerBasket?> GetBasketAsync(string basketId, CancellationToken ct = default);
        //Create or Update
        Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = default, CancellationToken ct = default);
        //Delete
        Task<bool> DeleteBasket(string basketId, CancellationToken ct = default);
    }
}
