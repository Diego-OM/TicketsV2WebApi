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
using System.Net.Sockets;

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

            var ticketGuid = string.Empty;
            var eventGuid = Guid.NewGuid().ToString();
            var clientGuid = Guid.NewGuid().ToString();

            Event events = new Event();
            events.EventID = eventGuid;
            events.EventName = payload.EventName;
            events.ClientID = clientGuid;

            _dbContext.Event.Add(events);

            await _dbContext.SaveChangesAsync();

            Tickets tickets = new Tickets();

            

            if (payload.TicketAmount > 1)
            {
                for (int i = 0; i < payload.TicketAmount; i++)
                {
                    
                    payload.ClientID = clientGuid;
                    payload.EventID = eventGuid;
                    payload.EventName = events.EventName;
                    payload.TicketID = Guid.NewGuid().ToString();
                
                    tickets.TicketID = payload.TicketID;
                    tickets.EventID = payload.EventID;
                    tickets.EventName = payload.EventName;
                    tickets.ClientID = payload.ClientID;
                    tickets.StatusID = "New";

                    tickets.QRCode = QRService.CreateTicket(payload);

                    _dbContext.Tickets.Add(tickets);
                    await _dbContext.SaveChangesAsync();

                }

                
            }
            else
            {
                payload.ClientID = clientGuid;
                payload.EventID = eventGuid;
                payload.EventName = events.EventName;
                payload.TicketID = Guid.NewGuid().ToString();

                tickets.TicketID = payload.TicketID;
                tickets.EventID = payload.EventID;
                tickets.EventName = payload.EventName;
                tickets.ClientID = payload.ClientID;
                tickets.StatusID = "New";

                tickets.QRCode = QRService.CreateTicket(payload);

                _dbContext.Tickets.Add(tickets);
                await _dbContext.SaveChangesAsync();
            }

            

            return new OkObjectResult(JsonConvert.SerializeObject("Tickets Generated"));

        }
    }
}
