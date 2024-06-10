using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics
{
    public interface IRiskSpecificDotInfoService
    {
        Task<string> UpdateDotInfoAsync(Guid riskId, RiskSpecificDotInfoDto model);
        Task<string> RemoveDotInfoAsync(Guid id);
    }
}
