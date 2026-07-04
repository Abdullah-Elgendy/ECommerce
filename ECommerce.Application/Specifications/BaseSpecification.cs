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
        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDesc { get; private set; }

        //Child classes that inherit BaseSpecification will be able to pass criteria to the constructor
        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        //Helper Methods to add expressions
        protected void AddInclude(Expression<Func<TEntity,Object>> includeExpression)
        {
            IncludeExpressions.Add(includeExpression);
        }
        protected void AddOrderBy(Expression<Func<TEntity,Object>> orderByExpression)
        {
            OrderBy = (orderByExpression);
        }
        protected void AddOrderByDesc(Expression<Func<TEntity,Object>> orderByDescExpression)
        {
            OrderByDesc = (orderByDescExpression);
        }

    }
}
