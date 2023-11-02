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
using System.Linq;
using TicketsV2.Models;
using System.Collections.Generic;

namespace TicketsV2
{

    public class GetTicketAmount
    {
        private readonly DBClient _dbContext;

        public GetTicketAmount(DBClient dBContext)
        {
            _dbContext = dBContext;
        }

        [FunctionName("GetTicketAmount")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get Ticket Amount Executed");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var payload = JsonConvert.DeserializeObject<Tickets>(requestBody);

            var query = new List<Tickets>();

            query = _dbContext.Tickets
                    .Where(t => t.ClientID == payload.ClientID).ToList();
            
            return new OkObjectResult(JsonConvert.SerializeObject(query.Count));
        }
    }
}

