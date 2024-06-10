using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration
{
    public interface IContentService
    {
        Task<ReportData> GetReportData(Guid riskId, bool isEndorsement = false);

        Task<Dictionary<string, List<dynamic>>> GetDataSets(Guid riskId);
    }
}