using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TicketsV2.Interfaces;
using TicketsV2.Services;
using System.Configuration;

[assembly: FunctionsStartup(typeof(TicketsV2.Startup))]

namespace TicketsV2
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

            builder.Services.AddDbContext<DBClient>(
              options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));
            
            builder.Services.AddSingleton<ITicket, QRService>();
        }
    }
}
