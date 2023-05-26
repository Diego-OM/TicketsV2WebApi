using System;
using System.IO;
using System.Threading.Tasks;
using TicketsV2.Services;
using TicketsV2.Interfaces;
using TicketsV2.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace TicketsV2
{
    public class CreateTickets
    {
        private readonly DBClient _dbContext;

        public CreateTickets(DBClient dBContext)
        {
            _dbContext = dBContext;
        }

        [FunctionName("CreateTickets")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Create Ticket Executed");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var payload = JsonConvert.DeserializeObject<Tickets>(requestBody);

            var ticketList = new List<string>();
           
            Event events = new Event();
            events.EventID = Guid.NewGuid().ToString();
            events.EventName = payload.EventName;
            
            events.ClientID = Guid.NewGuid().ToString();

            _dbContext.Event.Add(events);

            await _dbContext.SaveChangesAsync();

            Tickets tickets = new Tickets();

            if (payload.TicketAmount > 1)
            {
                for (int i = 0; i < payload.TicketAmount; i++)
                {
                    ticketList.Add(QRService.CreateTicket(payload));
                }
            }
            else
            {
                ticketList.Add(QRService.CreateTicket(payload));
            }

            foreach (var ticket in ticketList)
            {
                tickets.TicketID = Guid.NewGuid().ToString();
                tickets.EventID = events.EventID;
                tickets.EventName = events.EventName;
                tickets.ClientID = events.ClientID;
                tickets.StatusID = "New";

                tickets.QRCode = ticket;
                _dbContext.Tickets.Add(tickets);

                await _dbContext.SaveChangesAsync();
            }

            return new OkObjectResult(JsonConvert.SerializeObject("Tickets Generated"));

        }
    }
}
