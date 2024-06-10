using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IRiskPolicyContactService
    {
        Task<IEnumerable<RiskPolicyContactDTO>> GetByRiskDetailIdAsync(Guid riskDetailId);
        Task<RiskPolicyContactDTO> GetAsync(Guid id);
        Task<Guid> InsertAsync(RiskPolicyContactDTO model);
        Task<Guid> UpdateAsync(RiskPolicyContactDTO model);
        Task<bool> RemoveAsync(Guid id, bool fromEndorsement);
    }
}
