using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TicketsV2.Interfaces;
using TicketsV2.Services;

[assembly: FunctionsStartup(typeof(TicketsV2.Startup))]

namespace TicketsV2
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<DBClient>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, Environment.GetEnvironmentVariable("SqlConnectionString")));

            builder.Services.AddSingleton<ITicket, QRService>();
        }
    }
}
