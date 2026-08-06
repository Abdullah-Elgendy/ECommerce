using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Orders
{
    public class Order : BaseEntity<Guid>
    {
        #region Private Parameterless Constructor For EfCore
        private Order()
        {

        } 
        #endregion

        #region Constructor For Creating Object In Service
        public Order(string buyerEmail,
        OrderAddress shippingAddress,
        ICollection<OrderItem> items,
        DeliveryMethod deliveryMethod,
        decimal subTotal,
        string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            Items = items;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }
        #endregion

        public string PaymentIntentId { get; set; } = default!;
        public string BuyerEmail { get; set; } = default!;
        public OrderAddress ShippingAddress { get; set; } = default!;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public decimal SubTotal { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public int DeliveryMethodId { get; set; } //FK
        public decimal GetTotal() => SubTotal + (DeliveryMethod?.Cost ?? 0);
    }

}
