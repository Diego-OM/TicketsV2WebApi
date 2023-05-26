using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsV2.Models;

namespace TicketsV2.Interfaces
{
    public interface ITicket
    {
        internal string CreateTicket(Tickets ticket);

        internal void SaveTicket(List<string> ticketList);

    }
}
