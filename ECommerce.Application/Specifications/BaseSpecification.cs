using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Specifications
{
    internal abstract class BaseSpecification<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
        public Expression<Func<TEntity, bool>> Criteria { get; }

        //Helper Method to add expression to IncludeExpressions
        protected void AddInclude(Expression<Func<TEntity,Object>> include)
        {
            IncludeExpressions.Add(include);
        }

    }
}
