using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TicketsV2.Services;

namespace TicketsV2
{
    public static class GetTicketAmount
    {
        [FunctionName("GetTicketAmount")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get Ticket Amount Executed");

            BlobService blobService = new BlobService();

            var blobs = blobService.GetBlobs("borisbrejcha");

            
            
            return new OkObjectResult(JsonConvert.SerializeObject(""));
        }
    }
}

