using ECommerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Orders
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; } = default!;
        public AddressDto ShippingAddress { get; set; } = default!;
        public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public string DeliveryMethod { get; set; } = default!;
        public decimal SubTotal { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; } = default!;
        public decimal DeliveryMethodCost { get; set; } 
        //Auto mapper automatically maps GetTotal -> Total
        public decimal Total { get; set; }
        
    }
}
