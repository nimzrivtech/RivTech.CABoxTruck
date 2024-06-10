using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Fees
{
    public interface IFeeService
    {
        Task ResetServiceFeesAsync(Guid riskId);
    }

    public class FeeService : IFeeService
    {
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskCoverageRepository _riskCoverageRepository;

        public FeeService(
            IRiskRepository riskRepository,
            IRiskCoverageRepository riskCoverageRepository
        )
        {
            _riskRepository = riskRepository;
            _riskCoverageRepository = riskCoverageRepository;
        }

        public async Task ResetServiceFeesAsync(Guid riskId)
        {
            var risk = await _riskRepository.FindAsync(riskId);
            var forEndorRiskcov = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(risk.ForEndorsementRiskDetailId.Value);
            var lastIssuedRiskcov = await _riskCoverageRepository.GetRiskCoveragesAndPremiumsAsync(risk.CurrentRiskDetailId.Value);

            var forEndoCovPrem = forEndorRiskcov.First().RiskCoveragePremium;
            var lastIssuedCovPrem = lastIssuedRiskcov.First().RiskCoveragePremium;

            forEndoCovPrem.RiskMgrFeeAL = lastIssuedCovPrem.RiskMgrFeeAL;
            forEndoCovPrem.RiskMgrFeePD = lastIssuedCovPrem.RiskMgrFeePD;

            _riskCoverageRepository.Update(forEndorRiskcov.First());
            await _riskCoverageRepository.SaveChangesAsync();
        }
    }
}
