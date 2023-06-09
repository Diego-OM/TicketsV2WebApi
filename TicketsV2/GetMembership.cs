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
using static QRCoder.PayloadGenerator;

namespace TicketsV2
{
    public static class GetMembership
    {
        [FunctionName("GetMembership")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("StripeKey");

            string customerId = await new StreamReader(req.Body).ReadToEndAsync();

            var options = new CustomerGetOptions();
            var service = new CustomerService();
            options.AddExpand("subscriptions");
           
            var customer = service.Get("cus_O2ydWRgRvLIB7a", options);

            
            return new OkObjectResult(customer.Subscriptions.Data);
        }
    }
}

