using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface ICancellationBreakdownRepository : IRepository<CancellationBreakdown, long>
    {
        /// <summary>
        /// Returns the list of cancellation breakdowns based on given amount type.
        /// </summary>
        /// <param name="amountType"></param>
        /// <returns></returns>
        Task<IEnumerable<CancellationBreakdown>> GetAllByAmountTypeAsync(string amountType);
        IEnumerable<CancellationBreakdown> GetByRiskDetailId(Guid riskDetailId);
        Task<IEnumerable<CancellationBreakdown>> GetAllByRiskDetailIdAmountTypeAsync(Guid riskDetailId, string amountType);
    }
}
