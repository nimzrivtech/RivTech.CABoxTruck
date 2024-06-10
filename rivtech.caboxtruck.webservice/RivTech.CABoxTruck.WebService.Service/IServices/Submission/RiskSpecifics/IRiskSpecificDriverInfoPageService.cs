using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics
{
    public interface IRiskSpecificDriverInfoPageService
    {
        Task<string> UpdateDriverInfoAndHiringCriteriaAsync(Guid riskId, RiskSpecificDriverInfoPageDto model);
    }
}
