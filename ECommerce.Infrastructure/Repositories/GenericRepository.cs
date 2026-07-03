using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    internal class GenericRepository<TEntity, TKey>(DbContext context) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public void Add(TEntity entity) => context.Set<TEntity>().Add(entity);

        public void Remove(TEntity entity) => context.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => context.Set<TEntity>().Update(entity);

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default) => await context.Set<TEntity>().ToListAsync(ct);

        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default) => await context.Set<TEntity>().FindAsync(id, ct);

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.CreateQuery(context.Set<TEntity>(), spec);
            return await query.ToListAsync(ct);
        }
    }
}
