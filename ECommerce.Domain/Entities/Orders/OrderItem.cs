using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Orders
{
    public class OrderItem : BaseEntity<int>
    {
        public OrderedProduct Product { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        
    }
}
