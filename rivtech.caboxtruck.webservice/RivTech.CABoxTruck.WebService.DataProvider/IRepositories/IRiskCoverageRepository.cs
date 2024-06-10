using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IRiskCoverageRepository : IRepository<RiskCoverage, Guid>
    {
        Task<IEnumerable<RiskCoverage>> GetRiskCoveragesAndPremiumsAsync(Guid id);
        Task<RiskCoverage> GetRiskCoverageBySelectedBindOption(Guid riskDetailId);
        /// <summary>
        /// Returns RiskCoverage by option number and includes RiskCoveragePremium.
        /// </summary>
        Task<RiskCoverage> GetRiskCoverageByOption(Guid riskDetailId, int option);
        Task<IEnumerable<RiskDetail>> GetRiskDetailsAsync(Guid id);
    }
}
