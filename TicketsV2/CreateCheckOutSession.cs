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
        protected CreateCheckOutSession(){

        }

        [FunctionName("CreateCheckOutSession")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Create CheckOut Session Executed");

            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("StripeKey");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var parsed = JsonConvert.DeserializeObject<TicketsV2.Models.CheckOutSession>(requestBody);

            var productID = requestBody;

            var paymentUtility = new PaymentUtility();

            var origin = req.Headers["Referer"].ToString();

            var customerID = await paymentUtility.GetCustomerID(parsed.CustomerID);

            var membership = await paymentUtility.GetMembership(customerID);

            //this needs to validate if there is data, if not will fail
            var isMembershipActive = membership.Data[0].Status; //index out of bounds , empty array

            var customerPlan = await paymentUtility.GetPlanIDByCustomerID(customerID);

            if(customerPlan.Id == parsed.ProductID){
                return new OkObjectResult(JsonConvert.SerializeObject("Already Subscribed To This Tier"));
            }else{
                var subOptions = new SubscriptionListOptions { Customer = customerID };
                var service2 = new SubscriptionService(); 
                var subscriptions = service2.List(subOptions);

                var updateService = new SubscriptionService();
                
                
                var updateOptions = new SubscriptionUpdateOptions
                
                {
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Id = subscriptions.Data[0].Items.Data[0].Id,
                        Price = parsed.ProductID,
                    },
                },
                };

                updateService.Update(subscriptions.Data[0].Id, updateOptions);

                var options3 = new SessionCreateOptions
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
                var service3 = new SessionService();
                var session3 = await service3.CreateAsync(options3);

                req.HttpContext.Response.Headers.Add("Location", origin);

                return new OkObjectResult(JsonConvert.SerializeObject(session3.Url));
            }

            var priceID = customerPlan.Id;

            var accessLevel = paymentUtility.CheckAccesslevel(priceID);

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

