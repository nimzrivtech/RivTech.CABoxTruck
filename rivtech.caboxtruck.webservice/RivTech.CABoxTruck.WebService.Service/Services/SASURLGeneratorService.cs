using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs.Specialized;
using System.Security.Policy;
using System.Web;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class SASURLGeneratorService : ISASURLGeneratorService
    {
        private readonly IConfiguration _configuration;
        private readonly ILog4NetService _logger;
        public SASURLGeneratorService(
            IConfiguration configuration,
            ILog4NetService logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Uri GenerateSharedAccessSignatureURL(string url)
        {
            try
            {
                url = EncodeHashtag(url);

                var originalUri = new Uri(url);
                var azureConfiguration = _configuration.GetSection("AzureStorage");
                string accountName = azureConfiguration.GetValue<string>("AccountName");
                string accountKey = azureConfiguration.GetValue<string>("AccountKey");
                int SASURLExpirySeconds = azureConfiguration.GetValue<int>("SASURLExpirySeconds");

                StorageSharedKeyCredential storageSharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey);
                BlobClient blobClient = new BlobClient(originalUri, storageSharedKeyCredential);

                if (blobClient.CanGenerateSasUri)
                {
                    BlobSasBuilder sasBuilder = new BlobSasBuilder()
                    {
                        BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                        BlobName = blobClient.Name,
                        Resource = "b"
                    };

                    // Give access to the URL for 3 seconds
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddSeconds(SASURLExpirySeconds);
                    // Limit viewing to read-only
                    sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
                    Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
                    return sasUri;
                }
                else
                {
                    return originalUri;
                }
            }
            catch (Exception msg)
            {
                var errorMessage = $"There was an error generating a URI for the link: {url}";
                _logger.Error(errorMessage, msg.Message);
                throw new Exception("There was an error processing the URI for this resource");
            }

        }

        public Uri GenerateSharedAccessSignatureURL(string url, string newFilename)
        {
            try
            {
                url = EncodeHashtag(url);

                var originalUri = new Uri(url);
                var azureConfiguration = _configuration.GetSection("AzureStorage");
                string accountName = azureConfiguration.GetValue<string>("AccountName");
                string accountKey = azureConfiguration.GetValue<string>("AccountKey");
                int SASURLExpirySeconds = azureConfiguration.GetValue<int>("SASURLExpirySeconds");
                var encodedFilename = HttpUtility.UrlEncode(newFilename);

                StorageSharedKeyCredential storageSharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey);
                BlobClient blobClient = new BlobClient(originalUri, storageSharedKeyCredential);

                if (blobClient.CanGenerateSasUri)
                {
                    BlobSasBuilder sasBuilder = new BlobSasBuilder()
                    {
                        BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                        BlobName = blobClient.Name,
                        Resource = "b",
                        ContentDisposition = $"attachment;filename={encodedFilename}"
                };

                    // Give access to the URL for 3 seconds
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddSeconds(SASURLExpirySeconds);
                    // Limit viewing to read-only
                    sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
                    Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
                    return sasUri;
                }
                else
                {
                    return originalUri;
                }
            }
            catch (Exception msg)
            {
                var errorMessage = $"There was an error generating a URI for the link: {url}";
                _logger.Error(errorMessage, msg.Message);
                throw new Exception("There was an error processing the URI for this resource");
            }

        }

        private string EncodeHashtag(string url)
        {
            if (url.Contains('#'))
            {
                url = url.Replace("#", "%23");
            }

            return url;
        }
    }
}
