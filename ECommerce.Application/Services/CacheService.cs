using ECommerce.Application.Contracts;
using ECommerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    internal class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;
        public CacheService(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }
        public async Task<string?> GetDataAsync(string cacheKey, CancellationToken ct = default)
        => await _cacheRepository.GetAsync(cacheKey, ct);

        public async Task SetDataAsync(string cacheKey, object cacheValue, TimeSpan? duration = null, CancellationToken ct = default)
        {
            var serializedValue = JsonSerializer.Serialize(cacheValue, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase } );
            await _cacheRepository.SetAsync(cacheKey ,serializedValue, duration, ct);
        }
    }
}
