using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IRiskPolicyContactRepository : IRepository<RiskPolicyContact, Guid>
    {
        Task<IEnumerable<RiskPolicyContact>> GetAllIncludeAsync(Guid riskDetailId);
        Task<RiskPolicyContact> GetIncludeAsync(Guid id);
        Task<RiskPolicyContact> GetByRiskDetailIdIncludeAsync(Guid id, short typeId);
    }
}
