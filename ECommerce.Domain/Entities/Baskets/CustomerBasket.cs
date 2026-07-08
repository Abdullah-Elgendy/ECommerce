using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Baskets
{
    //will be in Memory Database
    public class CustomerBasket
    {
        public string Id { get; set; } = default!; //GUID
        public ICollection<BasketItem> Items { get; set; } = [];
 
    }
}
