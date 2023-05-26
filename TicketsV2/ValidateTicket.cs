using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TicketsV2.Models;
using TicketsV2.Services;

namespace TicketsV2
{
    public static class ValidateTicket
    {
        [FunctionName("ValidateTicket")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var qRCode = JsonConvert.DeserializeObject<QRCode>(requestBody);

            BlobService blobService = new BlobService();

            var blobs = await blobService.GetBlobs(qRCode.ClientID);


            foreach (var item in blobs)
            {
                var a = item;
            }


            return new OkObjectResult("");
        }
    }
}

