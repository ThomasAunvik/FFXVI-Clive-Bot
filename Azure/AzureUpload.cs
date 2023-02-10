using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Azure
{
    public class AzureUpload
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureUpload(BlobServiceClient blobClient) { 
            _blobServiceClient = blobClient;
        }

        public async Task<BlobContentInfo> Upload(
            string pathName, 
            Stream content, 
            string contentType,
            CancellationToken c = new()
        )   {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("$web");
            var blobClient = blobContainer.GetBlobClient(pathName);

            var blob = await blobClient.UploadAsync(
                content,
                options: new BlobUploadOptions()
                {
                    HttpHeaders = new BlobHttpHeaders() { 
                        ContentType = contentType,
                    },
                },
                cancellationToken: c
            );
            return blob.Value;
        }
    }
}
