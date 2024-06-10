using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskDetailRepository : BaseRepository<RiskDetail, Guid>, IRiskDetailRepository
    {
        public RiskDetailRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<IEnumerable<RiskDetail>> GetAllIncludeAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<RiskDetail> GetOneIncludeByIdAsync(Guid id)
        {
            var risk = await _context.RiskDetail
                                .Include(risk => risk.RiskDetailDrivers).ThenInclude(rdv => rdv.Driver).ThenInclude(config => config.Entity).AsSplitQuery()
                                .Include(risk => risk.RiskDetailDrivers).ThenInclude(rdv => rdv.Driver).ThenInclude(config => config.DriverIncidents).AsSplitQuery()
                                .Include(risk => risk.RiskDetailDrivers).ThenInclude(rdv => rdv.RiskDetail).AsSplitQuery()
                                .Include(risk => risk.RadiusOfOperations).AsSplitQuery()
                                .Include(risk => risk.Insured).ThenInclude(config => config.Entity)
                                    .ThenInclude(config => config.EntityAddresses).ThenInclude(config => config.Address).AsSplitQuery()
                                .Include(risk => risk.RiskFilings).AsSplitQuery()
                                .Include(risk => risk.DriverInfo).ThenInclude(driverInfo => driverInfo.MvrPullingFrequency).AsSplitQuery()
                                .Include(risk => risk.DriverHeader).AsSplitQuery()
                                .Include(risk => risk.DriverHiringCriteria).AsSplitQuery()
                                .Include(risk => risk.DotInfo).ThenInclude(dotInfo => dotInfo.ChameleonIssues).AsSplitQuery()
                                .Include(risk => risk.DotInfo).ThenInclude(dotInfo => dotInfo.CurrentSaferRating).AsSplitQuery()
                                .Include(risk => risk.DotInfo).ThenInclude(dotInfo => dotInfo.IssCabRating).AsSplitQuery()
                                .Include(risk => risk.UnderwritingQuestions)
                                .AsSplitQuery()
                                //.Include(risk => risk.Vehicles)
                                .Include(risk => risk.RiskDetailVehicles).ThenInclude(rdv => rdv.Vehicle)
                                .AsSplitQuery()
                                //.Include(risk => risk.RiskDetailVehicles).ThenInclude(rdv => rdv.Vehicle.VehicleProratedPremium)
                                .Include(risk => risk.RiskDetailVehicles).ThenInclude(rdv => rdv.RiskDetail)
                                .AsSplitQuery()
                                .Include(risk => risk.RiskCoverages)
                                    .ThenInclude(riskCoverage => riskCoverage.RiskCoveragePremium)
                                    .ThenInclude(riskCovPrem => riskCovPrem.RiskCoveragePremiumAvg)
                                .AsSplitQuery()
                                .Include(risk => risk.DriverHiringCriteria).ThenInclude(x => x.BackGroundCheckIncludes)
                                .AsSplitQuery()
                                .Include(risk => risk.DriverHiringCriteria).ThenInclude(x => x.DriverTrainingIncludes)
                                .AsSplitQuery()
                                .Include(risk => risk.CommoditiesHauled).AsSplitQuery()
                                //.Include(risk => risk.RiskResponses).ThenInclude(x => x.Question)
                                .FirstOrDefaultAsync(x => x.Id == id);

            return risk;
        }


        public async Task<RiskDetail> GetOneIncludeByIdNoTrackingAsync(Guid id)
        {
            var risk = await _context.RiskDetail
                            .Include(risk => risk.RiskDetailDrivers).ThenInclude(rdv => rdv.Driver).ThenInclude(config => config.Entity)
                            .Include(risk => risk.RiskDetailDrivers).ThenInclude(rdv => rdv.Driver).ThenInclude(config => config.DriverIncidents)
                            .Include(risk => risk.RiskDetailDrivers).ThenInclude(rdv => rdv.RiskDetail)
                            .Include(risk => risk.RadiusOfOperations)
                            .Include(risk => risk.Insured).ThenInclude(config => config.Entity)
                                .ThenInclude(config => config.EntityAddresses).ThenInclude(config => config.Address)
                            .Include(risk => risk.RiskFilings)
                            .Include(risk => risk.DriverInfo).ThenInclude(driverInfo => driverInfo.MvrPullingFrequency)
                            .Include(risk => risk.DriverHeader)
                            .Include(risk => risk.DriverHiringCriteria)
                            .Include(risk => risk.DotInfo).ThenInclude(dotInfo => dotInfo.ChameleonIssues)
                            .Include(risk => risk.DotInfo).ThenInclude(dotInfo => dotInfo.CurrentSaferRating)
                            .Include(risk => risk.DotInfo).ThenInclude(dotInfo => dotInfo.IssCabRating)
                            .Include(risk => risk.UnderwritingQuestions)
                            .AsSplitQuery()
                            .Include(risk => risk.RiskDetailVehicles).ThenInclude(rdv => rdv.Vehicle)
                            .AsSplitQuery()
                            .Include(risk => risk.RiskDetailVehicles).ThenInclude(rdv => rdv.RiskDetail)
                            .AsSplitQuery()
                            .Include(risk => risk.RiskCoverages).ThenInclude(riskCoverage => riskCoverage.RiskCoveragePremium)
                            .AsSplitQuery()
                            .Include(risk => risk.DriverHiringCriteria).ThenInclude(x => x.BackGroundCheckIncludes)
                            .AsSplitQuery()
                            .Include(risk => risk.DriverHiringCriteria).ThenInclude(x => x.DriverTrainingIncludes)
                            .AsSplitQuery()
                            .Include(risk => risk.CommoditiesHauled)
                            .FirstOrDefaultAsync(x => x.Id == id);
            return risk;
        }

        public async Task<RiskDetail> GetInsuredAsync(Guid riskId)
        {
            return await _context.Set<RiskDetail>()
                                .Include(i => i.Insured).ThenInclude(ti => ti.Entity)
                                .Where(x => x.Id == riskId).FirstOrDefaultAsync();
        }

        public async Task<RiskDetail> GetInsuredAddressAsync(Guid riskDetailId)
        {
            return await _context.Set<RiskDetail>()
                                .Include(i => i.Insured)
                                    .ThenInclude(ti => ti.Entity)
                                        .ThenInclude(config => config.EntityAddresses)
                                            .ThenInclude(config => config.Address)
                                .Where(x => x.Id == riskDetailId).FirstOrDefaultAsync();
        }

        public async Task<RiskDetail> GetInsuredAddressByRiskIdAsync(Guid riskId)
        {
            return await _context.Set<RiskDetail>()
                                .Include(i => i.Insured)
                                    .ThenInclude(ti => ti.Entity)
                                        .ThenInclude(config => config.EntityAddresses)
                                            .ThenInclude(config => config.Address)
                                .Where(x => x.RiskId == riskId).OrderByDescending(x => x.UpdatedDate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<VehiclePremiumRatingFactor>> GetVehicleRatingFactorsByRiskDetailId(Guid riskDetailId)
        {
            return await _context.Set<VehiclePremiumRatingFactor>().Where(vprf => vprf.RiskDetailId == riskDetailId).AsNoTracking().ToListAsync();
        }

        public async Task<RiskDetail> GetSubmissionRiskDetailAsync(Guid riskId)
        {
            IQueryable<PolicyHistory> policyHistories = _context.PolicyHistory.Where(x => x.RiskId == riskId.ToString());
            if (!policyHistories.Any())
                return await _context.RiskDetail.FirstOrDefaultAsync(x => x.RiskId == riskId);

            string riskDetailId = policyHistories.FirstOrDefault(x => x.EndorsementNumber == "0")?.PreviousRiskDetailId;
            if (riskDetailId is null)
                return null;
            return await GetOneIncludeByIdAsync(Guid.Parse(riskDetailId));
        }

        public async Task<RiskDetail> GetLatestRiskDetailByPolicyNumberAsync(Guid id)
        {
            var risk = await _context.RiskDetail
                                .Include(risk => risk.RiskDetailDrivers).ThenInclude(rdv => rdv.Driver).ThenInclude(config => config.Entity)
                                .Include(risk => risk.RiskDetailDrivers).ThenInclude(rdv => rdv.Driver).ThenInclude(config => config.DriverIncidents)
                                .Include(risk => risk.RiskDetailDrivers).ThenInclude(rdv => rdv.RiskDetail)
                                .Include(risk => risk.RadiusOfOperations)
                                .Include(risk => risk.Insured).ThenInclude(config => config.Entity)
                                    .ThenInclude(config => config.EntityAddresses).ThenInclude(config => config.Address)
                                .Include(risk => risk.RiskFilings)
                                .Include(risk => risk.DriverInfo).ThenInclude(driverInfo => driverInfo.MvrPullingFrequency)
                                .Include(risk => risk.DriverHeader)
                                .Include(risk => risk.DriverHiringCriteria)
                                .Include(risk => risk.DotInfo).ThenInclude(dotInfo => dotInfo.ChameleonIssues)
                                .Include(risk => risk.DotInfo).ThenInclude(dotInfo => dotInfo.CurrentSaferRating)
                                .Include(risk => risk.DotInfo).ThenInclude(dotInfo => dotInfo.IssCabRating)
                                .Include(risk => risk.UnderwritingQuestions)
                                .AsSplitQuery()
                                //.Include(risk => risk.Vehicles)
                                .Include(risk => risk.RiskDetailVehicles).ThenInclude(rdv => rdv.Vehicle)
                                .AsSplitQuery()
                                //.Include(risk => risk.RiskDetailVehicles).ThenInclude(rdv => rdv.Vehicle.VehicleProratedPremium)
                                .Include(risk => risk.RiskDetailVehicles).ThenInclude(rdv => rdv.RiskDetail)
                                .AsSplitQuery()
                                .Include(risk => risk.RiskCoverages).ThenInclude(riskCoverage => riskCoverage.RiskCoveragePremium)
                                .AsSplitQuery()
                                .Include(risk => risk.DriverHiringCriteria).ThenInclude(x => x.BackGroundCheckIncludes)
                                .AsSplitQuery()
                                .Include(risk => risk.DriverHiringCriteria).ThenInclude(x => x.DriverTrainingIncludes)
                                .AsSplitQuery()
                                .Include(risk => risk.CommoditiesHauled)
                                //.AsSplitQuery()
                                //.Include(risk => risk.RiskResponses).ThenInclude(x => x.Question)
                                .FirstOrDefaultAsync(x => x.Id == id);

            return risk;
        }
        public async Task<RiskDetail> GetLatestRiskDetailByRiskIdAsync(Guid? riskId)
        {
            var riskDetail = await _context.RiskDetail
                                .Include(risk => risk.Insured).ThenInclude(config => config.Entity).ThenInclude(config => config.EntityAddresses).ThenInclude(config => config.Address)
                                .OrderByDescending(o => o.EndorsementNumber).ThenByDescending(o => o.CreatedDate).FirstOrDefaultAsync(x => x.RiskId == riskId);

            return riskDetail;
        }

        public async Task<RiskDetail> GetFirstRiskDetailByRiskIdAsync(Guid? riskId)
        {
            var riskDetail = await _context.RiskDetail
                .Where(x => x.RiskId == riskId && x.EndorsementNumber == 0)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();

            return riskDetail;
        }

        public async Task<RiskDetail> GetLastRiskDetailByRiskIdAsync(Guid? riskId)
        {
            return await _context.RiskDetail
                                .Include(risk => risk.Insured).ThenInclude(config => config.Entity)
                                .OrderByDescending(o => o.UpdatedDate).Where(x => x.RiskId == riskId).Take(2).Reverse().FirstOrDefaultAsync();
        }

        public async Task<RiskDetail> GetLastRiskDetailByRiskIdAndCreatedDateAsync(Guid? riskId)
        {
            return await _context.RiskDetail
                                .Include(risk => risk.Insured).ThenInclude(config => config.Entity)
                                .OrderByDescending(o => o.CreatedDate)
                                .Where(x => x.RiskId == riskId && x.IsProcessed == true).FirstOrDefaultAsync();
        }

        public async Task<RiskDetail> GetRiskEndorsementPointsByRiskId(Guid riskId)
        {
            var riskDetail = await _context.RiskDetail
                .Where(r => r.RiskId.Equals(riskId) && r.IsProcessed == true && (r.Status == RiskDetailStatus.Active || r.Status == RiskDetailStatus.Quoted))
                .OrderByDescending(o => o.CreatedDate).FirstOrDefaultAsync();

            return riskDetail;
        }


        public async Task<int> GetNextVersionNumberOfEndorsementNumAsync(Guid riskId, int endorsementNumber)
        {
            var risk = await _context.Set<Risk>().FirstAsync(x => x.Id == riskId);
            var riskDetails = _context.RiskDetail.Where(x => x.RiskId == riskId
                                && x.EndorsementNumber == endorsementNumber
                                && x.Id != risk.ForEndorsementRiskDetailId);
            var latestVersion = riskDetails.Any() ? riskDetails.Max(x => x.VersionNumber) : -1;
            return await Task.FromResult(latestVersion + 1);
        }


        public async Task<Guid> GetLatestVersionAsync(Guid riskId)
        {
            var risk = await _context.Set<Risk>().FirstAsync(x => x.Id == riskId);
            return risk.ForEndorsementRiskDetailId.Value;
        }

        public async Task<Guid> GetRiskIdAsync(Guid riskDetailId)
        {
            var risk = await _context.Set<RiskDetail>().FirstAsync(x => x.Id == riskDetailId);
            return risk.RiskId;
        }
    }
}
