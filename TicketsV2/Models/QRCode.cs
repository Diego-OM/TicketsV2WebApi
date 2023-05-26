using System;
namespace TicketsV2.Models
{
	public class QRCode
	{
        public string TicketID { get; set; }
        public string EventID { get; set; }
        public string ClientID { get; set; }

        public QRCode()
		{
			
		}
	}
}

