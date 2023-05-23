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
using System.Collections.Generic;

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

            var ticketList = new List<string>();

            if(payload._TicketAmount > 1)
            {
                for (int i = 0; i < payload._TicketAmount; i++)
                {
                    ticketList.Add(QRService.CreateTicket(payload));
                }
            }
            else
            {
                ticketList.Add(QRService.CreateTicket(payload));
            }

            BlobService blobService = new BlobService();

            foreach (var ticket in ticketList)
            {
                blobService.UploadBlob($"{payload._EventName}", ticket);
            }

            return new OkObjectResult("Tickets Generated");

        }
    }
}
