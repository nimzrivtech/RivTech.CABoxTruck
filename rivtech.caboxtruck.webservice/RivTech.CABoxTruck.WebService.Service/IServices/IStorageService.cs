using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IStorageService
    {
        Task<string> Upload(IFormFile file, string fileName, string _CONTAINER_NAME = null, string _FOLDER_PATH = null);
        Task<string> UploadAR(IFormFile file, string fileName);
        Task<string> Upload(byte[] file, string fileName, string _CONTAINER_NAME = null, string _FOLDER_PATH = null);
        Task<string> Upload(Stream fileStream, string fileName, string _CONTAINER_NAME = null, string _FOLDER_PATH = null);

        Task<MemoryStream> Download(List<FileUploadDocumentDTO> documentDtos);
    }
}