using System;
namespace TicketsV2.Models
{
	public class BlobPayload
	{
		public Guid TicketID { get; set; }
		public Guid EventID { get; set; }
		public string EventName { get; set; }
		
	}
}

