using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using VideoWebAppApi.Interface;
using Microsoft.Extensions.Configuration;
using VideoWebAppApi.Models;
using Azure.Storage.Blobs.Models;

namespace VideoWebAppApi.Service
{
    public class AzureService : IAzureService
    {
        #region Dependency Injection / Constructor 
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly ILogger<AzureService> _logger;

        public AzureService(IConfiguration configuration, ILogger<AzureService> logger)
        {
            _storageConnectionString = configuration.GetValue<string>("BlobConnectionString");
            _storageContainerName = configuration.GetValue<string>("BlobContainerName");
            _logger = logger;
        }
        #endregion

        public string GenerateSasToken(string container, string blobName)
        {
            var blobServiceClient = new BlobServiceClient(_storageConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(container);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = container,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-1),
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(1)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Write | BlobSasPermissions.Create);

            // Generate the SAS token
            var sasToken = blobClient.GenerateSasUri(sasBuilder).Query;

            return sasToken;
        }
        public async Task<string> UploadFileToStorage(IFormFile file)
        {
            try
            {
                var containerClient = new BlobServiceClient(_storageConnectionString).GetBlobContainerClient(_storageContainerName);
                var blobClient = containerClient.GetBlobClient(file.FileName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
                }

                return blobClient.Uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while uploading to Azure Blob Storage: {ex.Message}");
                throw;
            }
        }

    }
}