using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics
{
    public interface IRiskSpecificDriverInfoService
    {
        Task<string> InsertDriverInfoAsync(RiskSpecificDriverInfoDto model);
        Task<List<RiskSpecificDriverInfoDto>> GetAllAsync();
        Task<RiskSpecificDriverInfoDto> GetDriverInfoAsync(Guid id);
        Task<string> UpdateDriverInfoAsync(RiskSpecificDriverInfoDto model);
        Task<string> RemoveDriverInfoAsync(Guid id);
    }
}
