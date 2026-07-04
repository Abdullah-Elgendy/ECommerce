using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Specifications
{
    internal class ProductCountSpecifications : BaseSpecification<Product, int>
    {
        public ProductCountSpecifications(ProductQueryParams queryParams) : base(P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId.Value)
            && (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId.Value)
            && (string.IsNullOrEmpty(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {

        }
    }
}
