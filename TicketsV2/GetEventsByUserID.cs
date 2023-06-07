using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TicketsV2.Models;
using TicketsV2.Services;
using System.Linq;

namespace TicketsV2
{
    public class GetEventsByUserID
    {
        private readonly DBClient _dbContext;

        public GetEventsByUserID(DBClient dBContext)
        {
            _dbContext = dBContext;
        }

        [FunctionName("GetEventsByUserID")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get Events By User ID");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var payload = JsonConvert.DeserializeObject<Client>(requestBody);

            var query = new List<Event>();

            query = _dbContext.Event
                    .Where(t => t.ClientID == payload.ClientID).ToList();

            var amount = query.Count();

            var result = new Tuple<List<Event>, int>(query, amount);

            return new OkObjectResult(result);
        }
    }
}

