using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRCoder;
using TicketsV2.Interfaces;
using TicketsV2.Models;

namespace TicketsV2.Services
{
    internal class QRService: ITicket
    {

        public QRService() 
        {
            
        }

        string ITicket.CreateTicket(Tickets ticket)
        {
           return CreateTicket(ticket);
        }

        void ITicket.SaveTicket(List<string> ticketList)
        {
            
        }


        internal static string CreateTicket(Tickets ticket)
        {
            string payload = JsonConvert.SerializeObject(ticket);

            var qrCodeGenerator = new QRCodeGenerator();

            var blobPayload = new BlobPayload() { ClientID = ticket.ClientID, TicketID = ticket.TicketID, EventID = ticket.EventID};

            var data = qrCodeGenerator.CreateQrCode(JsonConvert.SerializeObject(blobPayload), QRCodeGenerator.ECCLevel.Q);

            SvgQRCode qrCode = new SvgQRCode(data);
            
            return qrCode.GetGraphic(3);
        }

        
    }
}
