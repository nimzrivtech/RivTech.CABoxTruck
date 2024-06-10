using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class DriverRepository : BaseRepository<Driver, Guid>, IDriverRepository
    {
        public DriverRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<IEnumerable<Driver>> GetAllIncludeAsync()
        {
            return await _context.Set<Driver>()
                                .Include(config => config.DriverIncidents)
                                .Include(config => config.Entity)
                                .Include(config => config.RiskDetailDrivers)
                                    .ThenInclude(config => config.RiskDetail)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetAllIncludeAsync(Expression<Func<Driver, bool>> expression)
        {
            return await _context.Set<Driver>()
                                .Where(expression)
                                .Include(config => config.DriverIncidents)
                                .Include(config => config.Entity)
                                .ToListAsync();
        }

        public async Task<Driver> GetIncludeAsync(Expression<Func<Driver, bool>> expression)
        {
            return await _context.Set<Driver>()
                                .Where(expression)
                                .Include(config => config.DriverIncidents)
                                .Include(config => config.Entity)
                                .Include(config => config.RiskDetailDrivers)
                                    .ThenInclude(config => config.RiskDetail)
                                .FirstOrDefaultAsync();
        }

        public Driver GetById(Guid driverId)
        {
            return _context.Driver
                    .Include(v => v.RiskDetailDrivers)
                        .ThenInclude(v => v.RiskDetail)
                    .Include(v => v.DriverIncidents)
                    .Include(v => v.Entity)
                    .SingleOrDefault(v => v.Id == driverId);
        }

        public async Task<List<Driver>> GetByRiskDetailAllAsync(Guid riskDetailId)
        {
            return await _context.Driver
                .Include(v => v.RiskDetailDrivers)
                    .ThenInclude(v => v.RiskDetail)
                .Include(config => config.DriverIncidents)
                .Include(config => config.Entity)
                .Where(v => v.RiskDetailDrivers.Any(rdv => rdv.RiskDetailId == riskDetailId))
                ?.ToListAsync() ?? new List<Driver>();
        }

        public async Task<Driver> GetByRiskDetailAndIdAsync(Guid riskDetailId, Guid? driverId)
        {
            var driver = await _context.Driver
                .Where(v => v.Id == driverId).AsNoTracking()
                .Include(v => v.RiskDetailDrivers).AsNoTracking()
                .Where(v => v.RiskDetailDrivers.Any(rdv => rdv.RiskDetailId == riskDetailId)).AsNoTracking()
                .FirstOrDefaultAsync();
            return driver;
        }

        public async Task<Driver> GetByIdAsync(Guid driverId)
        {
            return await _context.Driver
                    .Include(v => v.RiskDetailDrivers)
                        .ThenInclude(v => v.RiskDetail)
                    .Include(v => v.DriverIncidents)
                    .Include(v => v.Entity)
                    .FirstOrDefaultAsync(v => v.Id == driverId);
        }

        public async Task<List<Driver>> GetByRiskDetailAsync(Guid riskDetailId)
        {
            return await _context.Driver
                .Include(v => v.RiskDetailDrivers)
                .Include(config => config.DriverIncidents)
                .Include(config => config.Entity)
                .Where(v => v.RiskDetailDrivers.Any(rdv => rdv.RiskDetailId == riskDetailId) && !v.DeletedDate.HasValue)
                ?.ToListAsync() ?? new List<Driver>();
        }

        public async Task<List<Driver>> GetPreviousRiskDetailDrivers(Guid riskId)

        {

            //var riskDetails = _context.RiskDetail.Where(rd => rd.RiskId == riskId)

            //    ?.OrderByDescending(x => x.UpdatedDate);

            //var previousRiskDetail = riskDetails?.Skip(1)?.Take(1).FirstOrDefault();

            //if (previousRiskDetail is null)

            //    return null;

            //return await GetByRiskDetailAsync(previousRiskDetail.Id);

            var risk = await _context.Set<Risk>().FindAsync(riskId);
            var previousRiskDetail = _context.RiskDetail.AsNoTracking().FirstOrDefault(x => x.Id == risk.CurrentRiskDetailId);
            if (previousRiskDetail is null)
                return null;
            return await GetByRiskDetailAsync(previousRiskDetail.Id);
        }

        public async Task<IEnumerable<Driver>> GetDriversByLastNameAndLicenseNumberAsync(string lastName, string licenseNumber)
        {
            return await _context.Set<Driver>()
                                .Include(config => config.DriverIncidents)
                                .Include(config => config.Entity)
                                .Include(config => config.RiskDetailDrivers)
                                    .ThenInclude(config => config.RiskDetail)
                                .Where(d => d.Entity.LastName.ToLower() == lastName.ToLower() && d.LicenseNumber.ToLower() == licenseNumber.ToLower())
                                .ToListAsync();
        }
    }

    public class DriverIncidentRepository : BaseRepository<DriverIncidents, Guid>, IDriverIncidentsRepository
    {
        public DriverIncidentRepository(AppDbContext context)
           : base(context)
        {
        }
    }

    public class DriverHeaderRepository : BaseRepository<DriverHeader, Guid>, IDriverHeaderRepository
    {
        public DriverHeaderRepository(AppDbContext context)
           : base(context)
        {
        }
    }
}


