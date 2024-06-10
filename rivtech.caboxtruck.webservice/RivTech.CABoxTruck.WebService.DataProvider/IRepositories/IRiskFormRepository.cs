using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IRiskFormRepository : IRepository<RiskForm, Guid>
    {
        Task UpdateRiskForm(RiskForm form);
        Task<List<RiskForm>> GetByIdIncludeAsync(Guid id);
    }
}