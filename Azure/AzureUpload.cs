using Azure.Storage.Blobs;
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

        public async Task Upload(string pathName, Stream content, CancellationToken c = new())
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("$web");
            var blobClient = blobContainer.GetBlobClient(pathName);

            await blobClient.UploadAsync(content, true, c);
        }
    }
}
