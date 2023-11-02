using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;
using Azure;
using System.Collections.Generic;
using Azure.Core;
using static System.Net.WebRequestMethods;
using Stripe.Terminal;
using System.Net;
using System.Net.Http;

namespace TicketsV2
{
    public class CreateCheckOutSession
    {
        [FunctionName("CreateCheckOutSession")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Create CheckOut Session Executed");

            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("StripeKey");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var productID = requestBody;

            var origin = req.Headers["Referer"].ToString();

            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{origin}success",
                CancelUrl = $"{origin}failure",
                CustomerEmail = "diego.ochoa.maldonado@hotmail.com",
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                Mode = "subscription",
                LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            Price = productID,
                            Quantity = 1,
                        }
                    }
            };
            var service = new SessionService();

            try
            {
                var session = await service.CreateAsync(options);

                req.HttpContext.Response.Headers.Add("Location", origin);
                return new OkObjectResult(JsonConvert.SerializeObject(session.Url));
            }
            catch (Exception ex)
            {
                return new OkObjectResult(ex.InnerException);
            }
        }
    }
}

