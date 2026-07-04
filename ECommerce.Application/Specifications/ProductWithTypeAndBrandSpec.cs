using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Specifications
{
    internal class ProductWithTypeAndBrandSpec : BaseSpecification<Product, int>
    {
        //Get All Products
        public ProductWithTypeAndBrandSpec(int? BrandId, int? TypeId)
            //BrandId Is Not Null -> P => P.BrandId == BrandId
            //TypeId Is Not Null -> P => P.TypeId == TypeId
            //TypeId And BrandId Are Not Null -> P.BrandId == BrandId && P.TypeId == TypeId
            : base(P => (!BrandId.HasValue || P.BrandId == BrandId.Value) && (!TypeId.HasValue || P.TypeId == TypeId.Value))
        //OR we can use
        //: base(P => (BrandId == null || P.BrandId == BrandId) && (TypeId == null || P.TypeId == TypeId))
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }

        //Get By Id
        public ProductWithTypeAndBrandSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

        }

    }
}
