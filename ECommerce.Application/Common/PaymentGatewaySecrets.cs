using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    public class PaymentGatewaySecrets
    {
        public string SecretKey { get; set; } = default!;
        public string DefaultCurrency { get; set; } = default!;

        public string WebhookSecret { get; set; } = default!;
    }
}
