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

        string ITicket.CreateTicket(Ticket ticket)
        {
           return CreateTicket(ticket);
        }

        void ITicket.SaveTicket(List<string> ticketList)
        {
            
        }


        internal static string CreateTicket(Ticket ticket)
        {
            string payload = JsonConvert.SerializeObject(ticket);

            var qrCodeGenerator = new QRCodeGenerator();

            var data = qrCodeGenerator.CreateQrCode("ENCODED TEXT", QRCodeGenerator.ECCLevel.Q);

            SvgQRCode qrCode = new SvgQRCode(data);
            
            return qrCode.GetGraphic(1);
        }

        internal static void SaveTicket(List<string> ticketList)
        {

        }

    }
}
