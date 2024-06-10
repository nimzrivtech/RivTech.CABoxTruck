using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class PolicyHistoryRepository : BaseRepository<PolicyHistory, long>, IPolicyHistoryRepository
    {
        public PolicyHistoryRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<PolicyHistory> GetFirstIssuance(Guid riskId)
        {
            var histories = await GetAsync(x => x.RiskId == riskId.ToString());
            return histories.Where(ph => ph.RiskId == riskId.ToString())
                    .OrderBy(ph => ph.EndorsementNumber)
                    .FirstOrDefault();
        }

        public async Task<PolicyHistory> GetLatestByRisk(Guid riskId)
        {
            var histories = await GetAsync(x => x.RiskId == riskId.ToString());
            return histories.Where(ph => ph.RiskId == riskId.ToString() && ph.PolicyStatus == DTO.Constants.RiskDetailStatus.Active)
                    .OrderByDescending(ph => ph.EndorsementNumber)
                    .FirstOrDefault();
        }

        public async Task<PolicyHistory> GetPreviousHistory(long id)
        {
            // Get history
            var history = await FindAsync(id);
            // Get all history of policy.
            var histories = _context.PolicyHistory.Where(x => x.RiskId == history.RiskId)?.ToList();
            // Sort histories by endorsement then by date created.
            //histories = histories.OrderBy(x => x.EndorsementNumber).ToList();
            histories = histories.OrderBy(x => x.EndorsementNumber).ThenBy(x => x.DateCreated).ToList();
            // Find index of history
            var index = histories.FindIndex(x => x == history);

            // return null if first history or non-existent histories list. 
            if (index <= 0)
                return null;

            // return the previous history.
            return histories.ToList()[index - 1];
        }

        public async Task<PolicyHistory> GetPreviousActiveHistory(long id)
        {
            // Get history
            var history = await FindAsync(id);
            // Get all history of policy.
            var histories = _context.PolicyHistory.Where(x => x.RiskId == history.RiskId)?.ToList();
            // Sort histories by endorsement then by date created.
            //histories = histories.OrderBy(x => x.EndorsementNumber).ToList();
            histories = histories.Where(x => x.PolicyStatus == RiskStatus.Active || x.Id == id)
                .OrderBy(x => x.EndorsementNumber).ThenBy(x => x.DateCreated).ToList();
            // Find index of history
            var index = histories.FindIndex(x => x == history);

            // return null if first history or non-existent histories list. 
            if (index <= 0)
                return null;

            // return the previous history.
            return histories.ToList()[index - 1];
        }


        public Task<PolicyHistory> GetLastActive(Guid riskId)
        {
            var histories = _context.PolicyHistory.Where(x => x.RiskId == riskId.ToString()).ToList();
            histories = histories.OrderByDescending(x => x.DateCreated).ToList();
            var history = histories.FirstOrDefault(x => x.PolicyStatus == RiskStatus.Active);
            return Task.FromResult(history);
        }
        public Task<PolicyHistory> GetLastCancelled(Guid riskId)
        {
            var histories = _context.PolicyHistory.Where(x => x.RiskId == riskId.ToString()).ToList();
            histories = histories.OrderByDescending(x => x.DateCreated).ToList();
            var history = histories.FirstOrDefault(x => x.PolicyStatus == RiskStatus.Canceled);
            return Task.FromResult(history);
        }

        public Task<IEnumerable<PolicyHistory>> GetByRiskDetailAsync(Guid riskDetailId)
        {
            return GetAsync(x => x.PreviousRiskDetailId == riskDetailId.ToString());
        }

        public Task<IEnumerable<PolicyHistory>> GetByRiskId(Guid riskId)
        {
            var result = _context.PolicyHistory.AsNoTracking().Where(x => x.RiskId == riskId.ToString()).ToList();
            return Task.FromResult(result.AsEnumerable());
        }
    }
}
