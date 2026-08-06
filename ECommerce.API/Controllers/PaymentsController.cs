using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace ECommerce.API.Controllers
{
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private readonly PaymentGatewaySecrets _paymentSettings;
        public PaymentsController(IPaymentService paymentService,
            IOptions<PaymentGatewaySecrets> paymentSettings,
            ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
            _paymentSettings = paymentSettings.Value;
        }

        #region POST CreateOrUpdatePaymentIntent
        [Authorize]
        [HttpPost("CreateOrUpdatePaymentIntent/{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId, CancellationToken ct = default)
            => ToActionResult(await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId, ct));
        #endregion

        #region POST Web Hook
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook(CancellationToken ct = default)
        {
            //Get Request body as a JSON object
            var requestJson = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                //Get Stripe signature from request json to create Stripe Event
                var stripeEvent = EventUtility.ConstructEvent(requestJson,
                    Request.Headers["Stripe-Signature"],
                    _paymentSettings.WebhookSecret);

                //get 'Data' from stripevent, then cast it as PaymentIntent using 'as' operator
                //if casting fails it will be null.
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:
                        if (paymentIntent != null)
                            await _paymentService.PaymentSucceededAsync(paymentIntent.Id, ct);
                        break;

                    case EventTypes.PaymentIntentPaymentFailed:
                        if (paymentIntent != null)
                            await _paymentService.PaymentFailedAsync(paymentIntent.Id, ct);
                        break;

                    default:
                        break;
                }

                return Ok();

            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
 
        }
        #endregion
    }

}
