using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IReinstatementBreakdownRepository : IRepository<ReinstatementBreakdown, long>
    {
        IEnumerable<ReinstatementBreakdown> GetByRiskDetailId(Guid riskDetailId);
    }
}
