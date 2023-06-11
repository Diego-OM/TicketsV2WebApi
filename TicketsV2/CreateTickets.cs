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
using Stripe;
using Microsoft.Extensions.Options;

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

            var reqBody = JsonConvert.DeserializeObject<EventsT>(requestBody);

            var paymentUtility = new PaymentUtility();

            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("StripeKey");

            var custInfo = new CustomerInformation();

            var customerInfo = custInfo.GetCustomerInformation(reqBody.ClientID);

            var customerId = paymentUtility.GetCustomerID(reqBody.ClientID);

            var membership = paymentUtility.GetMembership(customerId);

            var isMembershipActive = membership.Data[0].Status;

            var customerPlan = paymentUtility.GetPlanIDByCustomerID(customerId);

            var priceID = customerPlan.Id;

            var accessLevel = paymentUtility.CheckAccesslevel(priceID);

            var eventList = await _dbContext.Event.ToListAsync();

            var currentEventAmnt = eventList.Count;

            if(currentEventAmnt < accessLevel.EventAmount)
            {
                var qrPayload = new Tickets();

                var ticketList = new List<string>();

                var ticketGuid = string.Empty;
                var eventGuid = Guid.NewGuid().ToString();

                EventsT events = new EventsT();
                events.EventID = eventGuid;
                events.EventName = reqBody.EventName;
                events.ClientID = reqBody.ClientID;
                events.TicketAmount = reqBody.TicketAmount;

                _dbContext.Event.Add(events);

                await _dbContext.SaveChangesAsync();

                Tickets tickets = new Tickets();

                tickets.TicketAmount = reqBody.TicketAmount;

                if (tickets.TicketAmount > 1)
                {
                    for (int i = 0; i < reqBody.TicketAmount; i++)
                    {

                        qrPayload.ClientID = reqBody.ClientID;
                        qrPayload.EventID = eventGuid;
                        qrPayload.EventName = events.EventName;
                        qrPayload.TicketID = Guid.NewGuid().ToString();

                        tickets.TicketID = qrPayload.TicketID;
                        tickets.EventID = eventGuid;
                        tickets.EventName = qrPayload.EventName;
                        tickets.ClientID = qrPayload.ClientID;
                        tickets.StatusID = "New";

                        tickets.QRCode = QRService.CreateTicket(qrPayload);

                        _dbContext.Tickets.Add(tickets);
                        await _dbContext.SaveChangesAsync();

                    }


                }
                else
                {
                    qrPayload.ClientID = reqBody.ClientID;
                    qrPayload.EventID = eventGuid;
                    qrPayload.EventName = events.EventName;
                    qrPayload.TicketID = Guid.NewGuid().ToString();

                    tickets.TicketID = qrPayload.TicketID;
                    tickets.EventID = eventGuid;
                    tickets.EventName = qrPayload.EventName;
                    tickets.ClientID = qrPayload.ClientID;
                    tickets.StatusID = "New";

                    tickets.QRCode = QRService.CreateTicket(qrPayload);

                    _dbContext.Tickets.Add(tickets);
                    await _dbContext.SaveChangesAsync();
                }
            
        }
            else
            {
                return new OkObjectResult(JsonConvert.SerializeObject("Error While Creating Tickets"));
            }

            return new OkObjectResult(JsonConvert.SerializeObject("Tickets Generated"));

        }
    }
}
