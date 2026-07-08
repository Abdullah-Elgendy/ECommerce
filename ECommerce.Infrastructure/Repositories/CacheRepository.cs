using ECommerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    internal class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _context;
        public CacheRepository(IConnectionMultiplexer connection)
        {
            _context = connection.GetDatabase();
        }

        public async Task<string?> GetAsync(string cacheKey, CancellationToken ct = default)
        {
            var value = await _context.StringGetAsync(cacheKey);
            return value.IsNullOrEmpty ? null : value.ToString();
        }

        public async Task SetAsync(string cacheKey, string cacheValue, TimeSpan? duration = null, CancellationToken ct = default)
        {
            await _context.StringSetAsync(cacheKey, cacheValue, duration ?? TimeSpan.FromDays(2));

        }
    }
}
