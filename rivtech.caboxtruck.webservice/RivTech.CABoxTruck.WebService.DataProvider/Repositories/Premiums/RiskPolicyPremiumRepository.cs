using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Premiums
{
    public class RiskPolicyPremiumRepository : IRiskPolicyPremiumRepository
    {
        private readonly AppDbContext _dbContext;

        public RiskPolicyPremiumRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteAsync(RiskPolicyPremiumCollection riskPolicyPremiums)
        {
            _dbContext.ChangeTracker.Entries<RiskPolicyPremium>().ToList().ForEach(x => x.State = EntityState.Detached);
            _dbContext.ChangeTracker.Entries<PolicyCoveragePremium>().ToList().ForEach(x => x.State = EntityState.Detached);

            foreach (var premium in riskPolicyPremiums)
            {
                var existsInOther = await _dbContext.RiskPolicyPremium.AnyAsync(x => x.CoveragePremium.Id == premium.CoveragePremium.Id
                                        && x.RiskDetailId != premium.RiskDetailId);

                _dbContext.RiskPolicyPremium.Remove(premium);
                await _dbContext.SaveChangesAsync();

                if (!existsInOther)
                {
                    _dbContext.RiskPolicyPremiumCoverage.Remove(premium.CoveragePremium);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task<RiskPolicyPremiumCollection> GetByRiskDetailAsync(Guid riskDetailId, string premiumType, bool fromReinstatement = false)
        {
            var riskDetail = await _dbContext.RiskDetail.FindAsync(riskDetailId);
            if (riskDetail is null)
                return null;

            var brokerInfo = await _dbContext.BrokerInfo.AsNoTracking().FirstAsync(x => x.RiskId == riskDetail.RiskId);
            var policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
            var riskPolicyPremiums = _dbContext.RiskPolicyPremium.AsNoTracking()
                                    .Include(x => x.CoveragePremium).AsNoTracking()
                                    .Where(x => x.RiskDetailId == riskDetailId && x.PremiumType == premiumType && x.FromReinstatement == fromReinstatement)
                                    .ToList();

            //foreach (var premium in riskPolicyPremiums)
            //{
            //    premium.CoveragePremium.RecalculateProratedPremium(policyPeriod);
            //}

            var rvpColl = new RiskPolicyPremiumCollection(
                riskPolicyPremiums.OrderBy(x => x.Order),
                policyPeriod
            );

            return rvpColl;
        }

        public async Task<RiskPolicyPremiumCollection> GetCurrentAsync(Guid riskId, string premiumType)
        {
            var risk = await _dbContext.Set<Risk>().FindAsync(riskId);
            var latestRiskDetail = _dbContext.RiskDetail.First(x => x.Id == risk.CurrentRiskDetailId);
            return await GetByRiskDetailAsync(latestRiskDetail.Id, premiumType);
        }

        public async Task SaveAsync(RiskPolicyPremiumCollection riskPolicyPremiums)
        {
            foreach (var premium in riskPolicyPremiums)
            {
                var savedItem = _dbContext.RiskPolicyPremium.AsNoTracking()
                    .Include(x => x.CoveragePremium).AsNoTracking()
                    .FirstOrDefault(rvp => rvp.Id == premium.Id);

                if (savedItem is null)
                {
                    if (premium.CoveragePremium.Id != 0)
                    {
                        _dbContext.ChangeTracker.Entries<PolicyCoveragePremium>().ToList().ForEach(x => x.State = EntityState.Detached);
                        _dbContext.Attach(premium.CoveragePremium);
                    }
                    await _dbContext.RiskPolicyPremium.AddAsync(premium);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    _dbContext.ChangeTracker.Entries<RiskPolicyPremium>().ToList().ForEach(x => x.State = EntityState.Detached);
                    _dbContext.ChangeTracker.Entries<PolicyCoveragePremium>().ToList().ForEach(x => x.State = EntityState.Detached);

                    if (premium.CoveragePremium.Id != 0)
                    {
                        _dbContext.Entry(premium.CoveragePremium).State = EntityState.Detached;
                        _dbContext.Attach(premium.CoveragePremium);
                    }
                    _dbContext.RiskPolicyPremium.Update(premium);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateAsync(RiskPolicyPremiumCollection currentRiskPolicyPremiums)
        {
            foreach (var premium in currentRiskPolicyPremiums)
            {
                _dbContext.Update(premium);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
