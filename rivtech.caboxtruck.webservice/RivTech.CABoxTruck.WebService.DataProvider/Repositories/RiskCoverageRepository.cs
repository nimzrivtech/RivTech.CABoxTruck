using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Migrations;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class RiskCoverageRepository : BaseRepository<RiskCoverage, Guid>, IRiskCoverageRepository
    {
        public RiskCoverageRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<IEnumerable<RiskCoverage>> GetRiskCoveragesAndPremiumsAsync(Guid id)
        {
            return await _context.Set<RiskCoverage>()
                                .Include(riskCoverage => riskCoverage.RiskCoveragePremium)
                                .ThenInclude(riskCoveragePremium => riskCoveragePremium.RiskCoveragePremiumAvg)
                                .Where(riskCoverage => riskCoverage.RiskDetailId == id)
                                .ToListAsync();
        }


        public async Task<RiskCoverage> GetRiskCoverageBySelectedBindOption(Guid riskDetailId)
        {
            var rd = await _context.RiskDetail
                        .Include(x => x.RiskCoverages)
                        .FirstOrDefaultAsync(x => x.Id == riskDetailId);
            var binding = await _context.Set<Binding>()
                        .FirstOrDefaultAsync(x => x.RiskId == rd.RiskId);

            short optionId = (binding is null || binding.BindOptionId is null) 
                ? (short) 1 : binding.BindOptionId.Value;
            return rd.RiskCoverages.FirstOrDefault(x => x.OptionNumber == optionId);
        }
        public async Task<IEnumerable<RiskDetail>> GetRiskDetailsAsync(Guid id)
        {
            return await _context.Set<RiskDetail>()
                                .Include(riskDetail => riskDetail.RiskCoverages)
                                .ThenInclude(riskCoverage => riskCoverage.RiskCoveragePremium)
                                .ThenInclude(riskCoveragePremium => riskCoveragePremium.RiskCoveragePremiumAvg)
                                .Where(riskDetail => riskDetail.RiskId == id).OrderBy(x => x.EndorsementNumber)
                                .ToListAsync();
        }

        public async Task<RiskCoverage> GetRiskCoverageByOption(Guid riskDetailId, int option)
        {
            var rd = await _context.RiskDetail
                        .Include(x => x.RiskCoverages)
                            .ThenInclude(x => x.RiskCoveragePremium)
                        .FirstOrDefaultAsync(x => x.Id == riskDetailId);
            return rd.RiskCoverages.FirstOrDefault(x => x.OptionNumber == option);

        }
    }
}


