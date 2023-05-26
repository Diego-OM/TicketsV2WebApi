using System;
using System.ComponentModel.DataAnnotations;

namespace TicketsV2.Services
{
    public class Event
    {
        [Key]
        public string EventID { get; set; }
        public string EventName { get; set; }
        public string ClientID { get; set; }

    }
}