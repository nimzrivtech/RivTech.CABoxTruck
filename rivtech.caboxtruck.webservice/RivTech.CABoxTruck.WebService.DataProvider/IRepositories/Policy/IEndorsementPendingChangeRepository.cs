using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IEndorsementPendingChangeRepository : IRepository<EndorsementPendingChange, long>
    {
        IEnumerable<EndorsementPendingChange> GetByRiskDetail(Guid riskDetailId);
        void Delete(EndorsementPendingChange endorsementPendingChange);
        Task DeleteAsync(EndorsementPendingChange endorsementPendingChange);
        Task<IEnumerable<EndorsementPendingChange>> GetByRiskDetailAsync(Guid riskDetailId);
    }
}
