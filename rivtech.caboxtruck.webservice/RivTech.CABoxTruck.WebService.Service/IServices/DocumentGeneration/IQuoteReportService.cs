using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration
{
    public interface IQuoteReportService
    {
        public Task<string> GenerateQuoteReport(Guid RiskId);
        Task<string> GenerateSaveQuoteReport(Guid newRiskDetailId);
    }
}
