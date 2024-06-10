using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IRiskDetailRepository : IRepository<RiskDetail, Guid>
    {
        Task<RiskDetail> GetInsuredAsync(Guid riskId);
        Task<RiskDetail> GetInsuredAddressAsync(Guid riskId);
        Task<RiskDetail> GetInsuredAddressByRiskIdAsync(Guid riskId);
        Task<IEnumerable<RiskDetail>> GetAllIncludeAsync();
        Task<RiskDetail> GetOneIncludeByIdAsync(Guid id);
        Task<RiskDetail> GetOneIncludeByIdNoTrackingAsync(Guid id);
        Task<IEnumerable<VehiclePremiumRatingFactor>> GetVehicleRatingFactorsByRiskDetailId(Guid riskDetailId);

        Task<RiskDetail> GetSubmissionRiskDetailAsync(Guid riskId);
        Task<RiskDetail> GetLatestRiskDetailByRiskIdAsync(Guid? riskId);
        Task<RiskDetail> GetFirstRiskDetailByRiskIdAsync(Guid? riskId);
        Task<RiskDetail> GetLastRiskDetailByRiskIdAsync(Guid? riskId);
        Task<RiskDetail> GetLastRiskDetailByRiskIdAndCreatedDateAsync(Guid? riskId);
        Task<RiskDetail> GetRiskEndorsementPointsByRiskId(Guid riskId);
        Task<int> GetNextVersionNumberOfEndorsementNumAsync(Guid riskId, int endorsementNumber);
        Task<Guid> GetLatestVersionAsync(Guid riskDetailId);
        Task<Guid> GetRiskIdAsync(Guid riskDetailId);
    }
}
