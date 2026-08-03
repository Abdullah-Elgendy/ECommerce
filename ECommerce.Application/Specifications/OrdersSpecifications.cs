using ECommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Specifications
{
    internal class OrdersSpecifications : BaseSpecification<Order,Guid>
    {
        public OrdersSpecifications(string email) : base( x => x.BuyerEmail == email)
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.Items);
            AddOrderByDesc(x => x.OrderDate);
        }

        public OrdersSpecifications(Guid id, string email) : base(x => x.Id == id && x.BuyerEmail == email)
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.Items);
        }
    }
}
