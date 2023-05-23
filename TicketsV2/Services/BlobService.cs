using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
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

        internal async void UploadBlob(string eventName, string payload)
        {

            var blobContainerClient = GetBlobContainerClient(eventName);

            
            await blobContainerClient.UploadBlobAsync($"{eventName} {Guid.NewGuid()}", new BinaryData(payload));
        }
    }
}
