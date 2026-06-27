using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    internal class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            //check if _repositories dictionary already contains the repo or not and return it
            var typeName = typeof(TEntity).Name;
            if (_repositories.TryGetValue(typeName, out object? val))
            {
                //explicit cast from Object to IGenericRepository
                return (IGenericRepository<TEntity,TKey>)val;
            }
            else
            {
                //if it doesn't exist create a new repo and save it in the dictionary
                var repo = new GenericRepository<TEntity,TKey>(dbContext);
                _repositories[typeName] = repo;
                return (IGenericRepository<TEntity,TKey>)_repositories[typeName];
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
           return await dbContext.SaveChangesAsync(ct);
        }
    }
}
