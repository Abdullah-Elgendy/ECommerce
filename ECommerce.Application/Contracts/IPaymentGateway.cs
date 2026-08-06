using ECommerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    //This is the general interface that the external payment services in the infrastructure layer will follow
    //it doesn't matter which payment service it will be, for this project we will use Stripe.
    public interface IPaymentGateway
    {
        //Create PaymentIntent - Takes amount and currency type => returns object containing paymentIntentId and clientSecret
        Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default);

        //Update PaymentIntent - Takes updated amount and existing paymentIntentId => returns object containing updated paymentIntentId and updated clientSecret
        Task<PaymentIntentResult> UpdatePaymentIntentAsync(decimal amount, string paymentIntentId, CancellationToken ct = default);

    }
}
