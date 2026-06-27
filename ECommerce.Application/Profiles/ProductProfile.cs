using AutoMapper;
using ECommerce.Application.DTOs.Products;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Profiles
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType, TypeDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductBrand , opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType , opt => opt.MapFrom(src => src.ProductType.Name));
        }
    }
}
