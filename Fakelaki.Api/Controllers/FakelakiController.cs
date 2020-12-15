using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fakelaki.Api.Lib.DAL;
using Fakelaki.Api.Lib.Services.Interfaces;
using Fakelaki.Api.Lib.Models;
using Fakelaki.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using System.IO;
using Fakelaki.Api.Helpers;

namespace Fakelaki.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FakelakiController : ControllerBase
    {

        private readonly ILogger<FakelakiController> _logger;
        private IMapper _mapper;
        private readonly IFakelakiService _fakelakiService;
        private IUserService _userService;

        // Uncomment and replace with a real secret. You can find your endpoint's
        // secret in your webhook settings.
        private string webhookSecret = string.Empty;

        public FakelakiController(ILogger<FakelakiController> logger, IMapper mapper, IFakelakiService fakelakiService, IUserService userService, StripeSettings stripeSettings)
        {
            _logger = logger;
            _mapper = mapper;
            _fakelakiService = fakelakiService;
            _userService = userService;
            // Set your secret key. Remember to switch to your live secret key in production!
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.ApiKey = stripeSettings.ApiKey;
            webhookSecret = stripeSettings.WebhookSecret;
        }

        [HttpGet("{userId}")]
        public IActionResult GetAllEventFakelakia(int fakelakiId)
        {
            var fakelakia = _fakelakiService.GetByEvent(fakelakiId);
            var model = _mapper.Map<IList<FakelakiModel>>(fakelakia);
            return Ok(model);
        }

        [HttpPost("{eventId}/{emailTemplateId}")]
        public IActionResult Create([FromBody] FakelakiModel model, int emailTemplateId, int eventId)
        {
            // map model to entity

            if (model == null)
            {
                return BadRequest(new { message = "Fakelaki object is null." });
            }
            
            // set succesfull payment to false since it has not been completed yet
            model.SuccessfullPayment = false;

            var fakelaki = _mapper.Map<Lib.Models.Fakelaki>(model);

            try
            {

                var service = new PaymentIntentService();
                var createOptions = new PaymentIntentCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                      {
                        "card",
                      },
                    Amount = model.Amount,
                    Currency = "eur",
                    ApplicationFeeAmount = 100, //The fee that we get 1.00 EUR
                };

                var requestOptions = new RequestOptions();
                requestOptions.StripeAccount = _userService.GetAccountIdByEventid(eventId);


                PaymentIntent paymentIntent = service.Create(createOptions, requestOptions);
                fakelaki.PaymentIntentId = paymentIntent.Id;

                // create fakelaki
                _fakelakiService.Create(fakelaki, emailTemplateId, eventId);

                return Ok(new { client_secret = paymentIntent.ClientSecret });
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> ProcessWebhookEvent()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();


            // Verify webhook signature and extract the event.
            // See https://stripe.com/docs/webhooks/signatures for more information.
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], webhookSecret);

                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    var connectedAccountId = stripeEvent.Account;
                    HandleSuccessfulPaymentIntent(connectedAccountId, paymentIntent);
                    _fakelakiService.SetSuccessfullPayment(paymentIntent.Id);
                }

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.ToString());
                return BadRequest();
            }
        }

        private void HandleSuccessfulPaymentIntent(string connectedAccountId, PaymentIntent paymentIntent)
        {
            // Fulfill the purchase.
            _logger.LogInformation($"Connected account ID: {connectedAccountId}");
            _logger.LogInformation($"{paymentIntent}");
        }
    }
}
