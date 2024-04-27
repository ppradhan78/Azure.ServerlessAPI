using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using ServerlessAPI.Data.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ServerlessAPI.Data.Core.Blob
{
    public class FeedProcessBlobStorageCore : IFeedProcessBlobStorageCore
    {
        private readonly IConfiguration _configuration;

        public FeedProcessBlobStorageCore(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Uri> UploadFileBlobAsync(Stream content, string fileName)
        {
            try
            {
               // var containerClient = GetContainerClient(_configuration.StorageBlobContainerName);
                var containerClient = GetContainerClient(CommonConstants.blobContainerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                await blobClient.UploadAsync(content, overwrite: true);
                return blobClient.Uri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private BlobContainerClient GetContainerClient(string blobContainerName)
        {
            try
            {
                BlobServiceClient _blobServiceClient = new BlobServiceClient(CommonConstants.AzureStorageConnection);
                var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
                containerClient.CreateIfNotExists();
                return containerClient;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
