using RivTech.CABoxTruck.WebService.DTO.AzureFunctions;
using System.IO;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.AzureFunctions
{
    public interface IAzureFunctionApiService
    {
        Task<Stream> GenerateContentAsync(ContentGeneratorRequest request);
        //Task SendEmailAsync(SendEmailRequestDTO sendEmailRequest);
    }
}