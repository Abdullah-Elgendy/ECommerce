using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Baskets;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    internal class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase _context = connection.GetDatabase(); // In Memoery Database
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {

            //redis key = a string value
            //basket = object not a string, so we need to serialize 
            //it into a string using JsonSerializer.Serialize()
            var value = JsonSerializer.Serialize(basket);
            var res = await _context.StringSetAsync(basket.Id, value, timeToLive ?? TimeSpan.FromDays(7));

            return res ? basket : null;
        }

        public async Task<bool> DeleteBasket(string basketId, CancellationToken ct = default)
        {
            return await _context.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await _context.StringGetAsync(basketId);
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }
    }
}
