using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using TicketsV2.Models;

namespace TicketsV2.Services
{
    internal class BlobService
    {
        internal BlobContainerClient GetBlobContainerClient(string blobContainerName)
        {
            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");

            var blobContainerClient = new BlobContainerClient(connectionString, blobContainerName);


            blobContainerClient.CreateIfNotExists();

            return blobContainerClient;
        }
        
        internal async void UploadBlob(string clientID, string eventID, string payload)
        {
            string clientName = $"{clientID}-{eventID}";

            var blobContainerClient = GetBlobContainerClient(clientName);

            var ticketID = Guid.NewGuid();

            await blobContainerClient.UploadBlobAsync($"{ticketID}", new BinaryData(payload) );
        }

        internal async Task<List<string>> GetBlobs(string blobContainerName)
        {
            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");

            BlobContainerClient blobClient = new BlobContainerClient(connectionString, blobContainerName);

            var blobs = blobClient.GetBlobsAsync().AsPages();

            var blobList = new List<string>();


            await foreach (Page<BlobItem> blobPage in blobs)
            {
                foreach (BlobItem blobItem in blobPage.Values)
                {
                    blobList.Add(blobItem.Name);
                }

            }

            return blobList;
        }
    }
}
