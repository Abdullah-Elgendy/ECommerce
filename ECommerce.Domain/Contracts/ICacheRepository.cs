using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface ICacheRepository
    {
        public Task<string?> GetAsync(string cacheKey, CancellationToken ct = default);
        public Task SetAsync(string cacheKey, string cacheValue, TimeSpan? duration = default, CancellationToken ct = default);
    
    }
}
