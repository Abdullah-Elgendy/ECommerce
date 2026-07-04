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
            //dbContext.Set<TEntity>()
            var query = inputQuery;

            //add criteria if it's not null
            if(spec.Criteria != null)
            {
               query =  query.Where(spec.Criteria);
            }


            //add includes if any exist in specifications
            if (spec.IncludeExpressions.Any())
            {
                query = spec.IncludeExpressions.Aggregate(query, (current, nextExp) => current.Include(nextExp));
            }

            //add order by / order by desc if it's not null (order doesn't matter before or after include)
            if(spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if(spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }


            return query;
        }
    }
}
