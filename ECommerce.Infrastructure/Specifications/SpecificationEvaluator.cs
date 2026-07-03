using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Specifications
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, TKey> spec) where TEntity : BaseEntity<TKey> 
        {
            var query = inputQuery;

            //add includes if any exist in specifications
            if (spec.IncludeExpressions.Any())
            {
                //Method 1: Using ForEach()
                //    foreach (var expression in spec.IncludeExpressions)
                //    {
                //        query.Include(expression);
                //    }

                //Method 2: Using .Aggregate()
                query = spec.IncludeExpressions.Aggregate(query, (current, nextExp) => current.Include(nextExp));
            }

            return query;
        }
    }
}
