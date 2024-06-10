using Microsoft.AspNetCore.Http;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IFileUploadDocumentService
    {
        Task<List<FileUploadDocumentDTO>> GetAllAsync();
        Task<List<FileUploadDocumentDTO>> GetByRefIdAsync(Guid id);
        Task<List<FileUploadDocumentDTO>> GetByRiskDetailIdAsync(Guid id);
        Task<List<FileUploadDocumentDTO>> GetByRiskIdAsync(Guid id);

        Task<bool> IsExistAsync(Guid id);

        Task<FileUploadDocumentDTO> GetByIdAsync(Guid id);
        Task<List<FileUploadDocumentDTO>> GetByIdsAsync(List<Guid> ids);
        Task<FileUploadDocumentDTO> AddUpdateAsync(FileUploadDocumentDTO model);

        Task<string> RemoveAsync(Guid id);

        Task<FileUploadDocumentDTO> uploadAzure(Stream fileStream);
        Task<bool> CheckIsUploadedByIdAsync(Guid id);
        Task<bool> CheckIsAllowedEditDeleteAsync(Guid id);

        Task<bool> IsDocumentDuplicateFileNameAsync(Guid riskDetailId, List<string> fileNames, Guid? id);
        //Task<bool> IsBindReqDuplicateFileNameAsync(Guid riskDetailId, List<string> fileNames, Guid? id);
        Task<bool> IsBindQuoteDuplicateFileNameAsync(Guid riskDetailId, List<string> fileNames, Guid? id);
        byte[] CreateZipFile(IFormFileCollection formFiles);
        Task<FileUploadPagingDTO> GetByRiskIdAsync(FileUploadDocFilterDTO fileuploadfilter);
        Task<List<FileUploadDocumentDTO>> DownloadAllDocumentsAsync(FileUploadDocFilterDTO fileuploadfilter);
    }
}
