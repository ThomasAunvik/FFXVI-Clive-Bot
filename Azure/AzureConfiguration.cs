using Azure;
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

        public static Uri GetBlobServiceConnectionString(
           string accountName,
           string sasToken
        )
        {
            string blobUri = "https://" + accountName + ".blob.core.windows.net";

            return new Uri($"{blobUri}?{sasToken}");
        }

        public static IServiceCollection RegisterAzureBlobServices(
            this IServiceCollection serviceCollection,
            string blobAccountName,
            string blobSasToken
        )   {
            serviceCollection.AddScoped<AzureUpload>();
            serviceCollection.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddBlobServiceClient(
                    GetBlobServiceConnectionString(
                        blobAccountName,
                        blobSasToken
                    )
                );

                clientBuilder.UseCredential(new DefaultAzureCredential());
            });
            return serviceCollection;
        }
    }
}
