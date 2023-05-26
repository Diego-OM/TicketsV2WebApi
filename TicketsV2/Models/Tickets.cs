using System;
using System.ComponentModel.DataAnnotations;

namespace TicketsV2.Models
{
	public class Tickets
	{
        
        [Key]
        public string TicketID { get; set; }
        public string EventID { get; set; }
        public string EventName { get; set; }
        public string ClientID { get; set; }
        public string StatusID { get; set; }
        public int TicketAmount { get; set; }
        public string QRCode { get; set; }


    }
}

