using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Specifications
{
    internal class ProductWithIdSpecifications : BaseSpecification<Product, int>
    {
        public ProductWithIdSpecifications(HashSet<int> ProductIds) : base(p => ProductIds.Contains(p.Id))
        {
            
        }
    }
}
