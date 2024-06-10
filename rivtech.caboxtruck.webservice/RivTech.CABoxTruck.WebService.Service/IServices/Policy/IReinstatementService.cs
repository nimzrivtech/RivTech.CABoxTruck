using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Policy
{
    public interface IReinstatementService
    {
        Task ReverseCancellationAmounts(Guid riskId, Guid reinstatedRiskDetailId);
        Task<IEnumerable<ReinstatementBreakdown>> GetLatestAmountBreakdownsByRisk(Guid id);
    }
}