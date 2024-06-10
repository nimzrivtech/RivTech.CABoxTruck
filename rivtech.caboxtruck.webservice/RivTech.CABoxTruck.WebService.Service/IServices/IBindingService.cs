using Microsoft.AspNetCore.Http;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IBindingService
    {

        Task<BindingDTO> AddUpdateAsync(BindingDTO model);
        Task<BindingDTO> GetBindingByRiskIdAsync(Guid id);


        Task<List<BindingRequirementsDTO>> GetAllAsync();

        Task<bool> IsExistAsync(Guid id);

        Task<BindingRequirementsDTO> GetByIdAsync(Guid id);
        Task<List<BindingRequirementsDTO>> GetByRiskDetailIdAsync(Guid id);

        Task<BindingRequirementsDTO> AddUpdateAsync(BindingRequirementsDTO model);

        Task<string> RemoveAsync(Guid id);

        Task<List<BindingRequirementsDTO>> GetByRiskDetailIdIncludeAsync(Guid id);
        Task<List<BindingRequirementsDTO>> GetAllIncludeAsync();
        Task<List<BindingRequirementsDTO>> GetByIdIncludeAsync(Guid id);

        Task<List<BindingRequirementsDTO>> InsertRangeAsync(List<BindingRequirementsDTO> model);

        //Task<BindingRequirementsDTO> AzureBlogSave(IFormFileCollection formFiles, string States, BindingRequirementsDTO resource);
        Task<T> AzureBlogSave<T>(IFormFileCollection formFiles, string States, string type, T resource) where T : AzureBlobSaveDTO;
        Task<T> AzureBlogSave<T>(byte[] zipFile, string States, string type, T resource) where T : AzureBlobSaveDTO;

        Task<QuoteConditionsDTO> AddUpdateAsync(QuoteConditionsDTO model);

        Task<List<QuoteConditionsDTO>> GetQuoteByRiskDetailIdAsync(Guid id);
        Task<int[]> GetQuoteConditionOptionIsExist(Guid id, int[] quoteConditionOptionIds);
        Task<QuoteConditionsDTO> GetQuoteByIdAsync(Guid id);
        Task<string> RemoveQuoteAsync(Guid id);
        Task<List<QuoteConditionsDTO>> SetQuoteConditionOptions(QuoteConditionRequestDTO quoteConditionRequest);
    }
}
