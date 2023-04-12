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
using TicketsV2.Interfaces;
using TicketsV2.Models;

namespace TicketsV2
{
    public class CreateTickets
    {
        [FunctionName("CreateTickets")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Create Ticket Executed");
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var payload = JsonConvert.DeserializeObject<Ticket>(requestBody);

            var ticket = QRService.CreateTicket(payload);

            return new OkObjectResult(ticket);

        }
    }
}
