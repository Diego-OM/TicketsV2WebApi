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
        private static QRCodeGenerator _qrWriter;
        private static QRCodeData _qrCodeData;

        public QRService() 
        {
            _qrWriter = new QRCodeGenerator();
        }

        string ITicket.CreateTicket(Ticket ticket)
        {
           return CreateTicket(ticket);
        }

        internal static string CreateTicket(Ticket ticket)
        {
            string payload = JsonConvert.SerializeObject(ticket);

            _qrCodeData = _qrWriter.CreateQrCode("ENCODED TEXT", QRCodeGenerator.ECCLevel.Q);

            SvgQRCode qrCode = new SvgQRCode(_qrCodeData);
            
            return qrCode.GetGraphic(1);
        }


    }
}
