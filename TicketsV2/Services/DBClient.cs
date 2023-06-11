using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsV2.Models;

namespace TicketsV2.Services
{
	public class DBClient : DbContext
	{
		public DBClient(DbContextOptions<DBClient> options) : base(options)
		{
			
		}

		public DbSet<EventsT> Event { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
    }
}



