using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsV2.Interfaces;

namespace TicketsV2.Models
{
    sealed internal class Ticket
    {
        private readonly Guid _TicketID;
        internal string _EventName;
        private DateTime _EventDate;
        private DateTime _EventStartTime;
        private DateTime _EventEndTime;
        private DateTime _TicketExpiration;
        internal int _TicketAmount;

        public Ticket(string EventName, DateTime EventDate, DateTime EventStartTime, DateTime EventEndTime, DateTime TicketExpiration,
            int TicketAmount)
        {
            _TicketID = Guid.NewGuid();
            _EventName = EventName;
            _EventDate = EventDate;
            _EventStartTime = EventStartTime;
            _EventEndTime = EventEndTime;
            _TicketExpiration = TicketExpiration;
            _TicketAmount = TicketAmount;
        }
    }
}
