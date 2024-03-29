﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TicketsV2.Services
{
    public class EventsT
    {
        [Key]
        public string EventID { get; set; }
        public string ClientID { get; set; }
        public string ClientName { get; set; }
        public string EventName { get; set; }
        public int TicketAmount { get; set; }
    }
}