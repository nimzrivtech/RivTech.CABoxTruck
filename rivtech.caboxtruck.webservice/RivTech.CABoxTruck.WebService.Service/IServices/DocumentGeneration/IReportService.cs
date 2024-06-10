using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration
{
    public interface IReportService
    {
        Task<string> GenerateFileReportLink(ReportPayload payload);
    }
}
