using AutoMapper;
using ECommerce.Application.DTOs.Identity;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Profiles
{
    internal class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDto, OrderAddress>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(x => x.DeliveryMethod, opt => opt.MapFrom(y => y.DeliveryMethod.ShortName))
                .ForMember(x => x.DeliveryMethodCost, opt => opt.MapFrom(y => y.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(x => x.PictureUrl, opt => opt.MapFrom<OrderPictureUrlResolver>())
                .AfterMap(
                (item, itemDto) =>
                {
                    itemDto.ProductId = item.Product.ProductId;
                    itemDto.ProductName = item.Product.ProductName;
                });

            CreateMap<DeliveryMethod, DeliveryMethodDto>();

        }
    }
}
