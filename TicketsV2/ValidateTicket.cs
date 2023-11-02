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
using System.Collections.Generic;
using System.Linq;

namespace TicketsV2
{
    public  class ValidateTicket
    {
        private readonly DBClient _dbContext;

        public ValidateTicket(DBClient dBContext)
        {
            _dbContext = dBContext;
        }

        [FunctionName("ValidateTicket")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var payload = JsonConvert.DeserializeObject<Tickets>(requestBody);

            var query = new List<Tickets>();

            var result = string.Empty;

            query = _dbContext.Tickets
                    .Where(t => t.ClientID == payload.ClientID && t.TicketID == payload.TicketID).ToList();

            if(query.Count == 1)
            {
                result = "Ticket Valido";
            }
            else
            {
                result = "Ticket No Valido";
            }
            
            return new OkObjectResult(JsonConvert.SerializeObject(result));
        }
    }
}

