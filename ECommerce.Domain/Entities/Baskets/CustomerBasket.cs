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

        #region For Payment Module
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; } 
        #endregion
    }
}
