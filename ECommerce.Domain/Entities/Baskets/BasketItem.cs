using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Baskets
{

    //we won't inherit from base entity
    //because this class will NOT be mapped in the database
    //it will be stored in Memory Database
    public class BasketItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
