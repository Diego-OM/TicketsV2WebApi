﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stripe;

namespace TicketsV2
{
    public static class GetCustomerID
    {
        [FunctionName("GetCustomerID")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("StripeKey");

            string customerEmail = await new StreamReader(req.Body).ReadToEndAsync();

            var options = new CustomerListOptions();
            options.Email = customerEmail;

            var service = new CustomerService();

            var res = service.List(options);

            return new OkObjectResult(res.Data);
        }
    }
}

