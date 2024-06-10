using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission.RiskSpecifics
{
    public class RiskSpecificDriverHiringCriteriaRepository : BaseRepository<RiskSpecificDriverHiringCriteria, Guid>, IRiskSpecificDriverHiringCriteriaRepository
    {
        private readonly AppDbContext _context;
        public RiskSpecificDriverHiringCriteriaRepository(AppDbContext context)
           : base(context)
        {
            _context = context;
        }

        public async Task<RiskSpecificDriverHiringCriteria> GetWithIncludesNoTracking(Guid id)
        {
            return await _context.Set<RiskSpecificDriverHiringCriteria>()
                .Include(x => x.BackGroundCheckIncludes).AsNoTracking()
                .Include(x => x.DriverTrainingIncludes).AsNoTracking()
                .Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<BackgroundCheckOption>> GetBackgroundCheckOptions(Guid id)
        {
            var driverHiringCriteria = await _context.Set<RiskSpecificDriverHiringCriteria>().Include(x => x.BackGroundCheckIncludes)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (driverHiringCriteria != null)
            {
                return driverHiringCriteria.BackGroundCheckIncludes;
            }
            return null;
        }

        public async Task<List<DriverTrainingOption>> GetDriverTrainingOptions(Guid id)
        {
            var driverHiringCriteria = await _context.Set<RiskSpecificDriverHiringCriteria>().Include(x => x.DriverTrainingIncludes)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (driverHiringCriteria != null)
            {
                return driverHiringCriteria.DriverTrainingIncludes;
            }
            return null;
        }
    }
}
