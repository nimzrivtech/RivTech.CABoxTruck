using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class AzureStorageService : IStorageService
    {
        private const string CONTAINER_NAME = "risk-document";
        private const string FOLDER_PATH = "user-upload";
        private readonly IConfiguration _config;

        public AzureStorageService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> Upload(IFormFile file, string fileName, string _CONTAINER_NAME = null, string _FOLDER_PATH = null)
        {
            var connectionString = _config.GetConnectionString("AzureStorage");

            // Create a BlobServiceClient object which will be used to create a container client
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_CONTAINER_NAME ?? CONTAINER_NAME);

            // Get a reference to a blob
            var blobClient = containerClient.GetBlobClient($"{_FOLDER_PATH ?? FOLDER_PATH}/{fileName}");

            var fileStream = file.OpenReadStream();
            var blobHttpHeader = new BlobHttpHeaders {ContentType = file.ContentType};

            await blobClient.UploadAsync(fileStream, blobHttpHeader);
            fileStream.Close();

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<string> UploadAR(IFormFile file, string fileName)
        {
            var connectionString = _config.GetConnectionString("AzureStorage");

            // Create a BlobServiceClient object which will be used to create a container client
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(CONTAINER_NAME);

            // Get a reference to a blob
            var blobClient = containerClient.GetBlobClient($"{fileName}");

            var fileStream = file.OpenReadStream();
            var blobHttpHeader = new BlobHttpHeaders { ContentType = file.ContentType };

            await blobClient.UploadAsync(fileStream, blobHttpHeader);
            fileStream.Close();

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<string> Upload(byte[] file, string fileName, string _CONTAINER_NAME = null, string _FOLDER_PATH = null)
        {
            var connectionString = _config.GetConnectionString("AzureStorage");

            // Create a BlobServiceClient object which will be used to create a container client
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_CONTAINER_NAME ?? CONTAINER_NAME);

            // Get a reference to a blob
            var blobClient = containerClient.GetBlobClient($"{_FOLDER_PATH ?? FOLDER_PATH}/{fileName}");

            var blobHttpHeader = new BlobHttpHeaders();
            using (var fileStream = new System.IO.MemoryStream(file))
            {
                await blobClient.UploadAsync(fileStream, blobHttpHeader);
            }

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<string> Upload(Stream fileStream, string fileName, string _CONTAINER_NAME = null, string _FOLDER_PATH = null)
        {
            var connectionString = _config.GetConnectionString("AzureStorage");

            // Create a BlobServiceClient object which will be used to create a container client
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_CONTAINER_NAME ?? CONTAINER_NAME);

            // Get a reference to a blob
            var blobClient = containerClient.GetBlobClient($"{_FOLDER_PATH ?? FOLDER_PATH}/{fileName}");

            var blobHttpHeader = new BlobHttpHeaders();
            await blobClient.UploadAsync(fileStream, blobHttpHeader);

            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<MemoryStream> Download(List<FileUploadDocumentDTO> documentDtos)
        {
            var outputStream = new MemoryStream();
            var zipStream = new ZipOutputStream(outputStream);
            zipStream.SetLevel(0);

            var connectionString = _config.GetConnectionString("AzureStorage");
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(CONTAINER_NAME);

            var index = 1;
            foreach (var documentDto in documentDtos)
            {
                var startIndex = documentDto.FilePath.IndexOf(CONTAINER_NAME, StringComparison.Ordinal);
                var blobName = HttpUtility.UrlDecode(documentDto.FilePath.Substring(startIndex + CONTAINER_NAME.Length + 1));

                var blobClient = containerClient.GetBlobClient(blobName);

                await using (var blobStream = new MemoryStream())
                {
                    var newEntry = new ZipEntry($"{index} - {documentDto.FileName}") { DateTime = DateTime.Now };

                    zipStream.PutNextEntry(newEntry);

                    await blobClient.DownloadToAsync(blobStream);
                    blobStream.Position = 0;
                    await blobStream.CopyToAsync(zipStream);

                    zipStream.CloseEntry();
                    blobStream.Close();
                }

                index++;
            }

            zipStream.IsStreamOwner = false; 
            zipStream.Close();
            outputStream.Seek(0, SeekOrigin.Begin);

            return outputStream;
        }
    }
}