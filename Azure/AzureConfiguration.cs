using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Azure
{
    public static class AzureConfigurationExtensions
    {
        public static void GetBlobServiceClientSAS(
            ref BlobServiceClient blobServiceClient,
            string accountName,
            string sasToken
        )
        {
            string blobUri = "https://" + accountName + ".blob.core.windows.net";

            blobServiceClient = new BlobServiceClient
            (new Uri($"{blobUri}?{sasToken}"), null);
        }

        public static string GetBlobServiceConnectionString(
           string accountName,
           string sasToken
        )
        {
            if(sasToken.StartsWith("?")) sasToken = sasToken[1..];

            string blobUri = "BlobEndpoint=https://" + accountName + ".blob.core.windows.net;";
            string sas = "SharedAccessSignature=" + sasToken;
            return blobUri + sas;
        }

        public static IServiceCollection RegisterAzureBlobServices(
            this IServiceCollection serviceCollection,
            string blobConnectionString
        )   {
            serviceCollection.AddScoped<AzureUpload>();
            serviceCollection.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddBlobServiceClient(blobConnectionString);
            });
            return serviceCollection;
        }
    }
}
