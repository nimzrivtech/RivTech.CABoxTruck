using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class VehicleRepository : BaseRepository<Vehicle, Guid>, IVehicleRepository
    {
        public VehicleRepository(AppDbContext context)
           : base(context)
        {
        }

        public async Task<IEnumerable<Vehicle>> GetAllIncludeAsync()
        {
            return await _context.Set<Vehicle>()
                                //.Include(config => config.driverSummary)
                                //.Include(config=> config.driverPeriod)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAllIncludeAsync(Expression<Func<Vehicle, bool>> expression)
        {
            return await _context.Set<Vehicle>()
                                .Where(expression)
                                //.Include(config => config.driverSummary)
                                //.Include(config => config.driverPeriod)
                                .ToListAsync();
        }

        public Vehicle GetById(Guid vehicleId)
        {
            return _context.Vehicle
                    //.Include(v => v.VehicleProratedPremium)
                    .Include(v => v.RiskDetailVehicles)
                        .ThenInclude(v => v.RiskDetail)
                    .Include(v => v.PreviousVehicleVersion)
                        .ThenInclude(pvv => pvv.RiskDetailVehicles)
                            .ThenInclude(rdv => rdv.RiskDetail)
                    .SingleOrDefault(v => v.Id == vehicleId);
        }

        public async Task<Vehicle> GetByIdAsync(Guid vehicleId)
        {
            return await _context.Vehicle
                    .Include(v => v.RiskDetailVehicles)
                        .ThenInclude(v => v.RiskDetail).AsSplitQuery()
                    .Include(v => v.PreviousVehicleVersion)
                        .ThenInclude(pvv => pvv.RiskDetailVehicles)
                            .ThenInclude(rdv => rdv.RiskDetail).AsSplitQuery()
                    .SingleOrDefaultAsync(v => v.Id == vehicleId);
        }


        public async Task<IEnumerable<Vehicle>> GetByRiskDetailAsync(Guid riskDetailId)
        {
            var vehicles = await _context.Vehicle
                .Include(v => v.PreviousVehicleVersion)
                .Include(v => v.RiskDetailVehicles)
                    .ThenInclude(rdv => rdv.RiskDetail)
                .Where(v => v.RiskDetailVehicles.Any(rdv => rdv.RiskDetailId == riskDetailId))
                ?.ToListAsync() ?? new List<Vehicle>();

            var filteredVehicles = vehicles
                .OrderBy(x => x.VIN)
                .Where(v => !v.IsDeleted);

            return filteredVehicles;
        }


        public async Task<IEnumerable<Vehicle>> GetByRiskDetailAsync(Guid riskDetailId, bool? fromPolicy = null, short? optionNumber = 1)
        {
            var vehicles = await _context.Vehicle
                .Where(v => v.RiskDetailVehicles.Any(rdv => rdv.RiskDetailId == riskDetailId))
                .Include(v => v.PreviousVehicleVersion).AsSplitQuery()
                .Include(v => v.RiskDetailVehicles)
                    .ThenInclude(rdv => rdv.RiskDetail).AsSplitQuery()
                .ToListAsync();

            vehicles = vehicles
                .Where(v =>
                    v.IsDeleted != true
                    || (fromPolicy == true && v.IsDeleted == true)
                ) 
                .OrderBy(x => x.VIN)
                .ToList();

            foreach (var veh in vehicles)
            {
                await AssignPremiums(veh, riskDetailId, fromPolicy ?? false, optionNumber ?? 1);
            }

            return vehicles;
        }

        public async Task<Vehicle> GetByRiskDetailAndMainIdAsync(Guid riskDetailId, Guid mainId)
        {
            var vehicle = await _context.Vehicle
                .Where(v => v.MainId == mainId)
                .Include(v => v.RiskDetailVehicles)
                .Where(v => v.RiskDetailVehicles.Any(rdv => rdv.RiskDetailId == riskDetailId))
                .FirstOrDefaultAsync();
            return vehicle;
        }

        private async Task AssignPremiums(Vehicle vehicle, Guid riskDetailId, bool fromPolicy, short optionNumber)
        {
            int option = optionNumber;
            if (fromPolicy)
            {
                option = 0;

                /*
                var query = from rd in _context.RiskDetail
                            join bind in _context.Set<Binding>() on rd.RiskId equals bind.RiskId
                            where rd.Id == riskDetailId
                            select bind;
                option = query.FirstOrDefault()?.BindOptionId ?? 1;
                */
            }
            vehicle.VehiclePremiumRatingFactor = await _context.VehiclePremiumRatingFactor.AsNoTracking()
                .FirstOrDefaultAsync(v => v.VehicleId == vehicle.Id.ToString() && v.RiskDetailId == riskDetailId && v.OptionId == option);
        }

        public async Task<IEnumerable<Vehicle>> GetPreviousVehiclesByRiskDetail(RiskDetail riskDetail, bool? fromPolicy = null)
        {
            var risk = await _context.Set<Risk>().FindAsync(riskDetail.RiskId);
            var previousRiskDetail = _context.RiskDetail.AsNoTracking().FirstOrDefault(x => x.Id == risk.CurrentRiskDetailId);
            if (previousRiskDetail is null)
                return null;
            return await GetByRiskDetailAsync(previousRiskDetail.Id, fromPolicy);
        }


        public async Task<bool> IsFromSubmission(Guid vehicleId)
        {
            Vehicle vehicle = GetById(vehicleId);
            IQueryable<PolicyHistory> policyHistory = _context.PolicyHistory.Where(x => x.RiskId == vehicle.RiskDetail.RiskId.ToString());
            string submissionRiskDetailId = policyHistory.FirstOrDefault(x => x.EndorsementNumber == "0")?.PreviousRiskDetailId ?? null;
            var vehicles = await GetByRiskDetailAsync(Guid.Parse(submissionRiskDetailId), true);
            return vehicles.Any(v => v.MainId == vehicle.MainId);
        }

        public bool IsOldVersion(Guid? id)
        {
            var query = from rdv in _context.RiskDetailVehicle
                        join rd in _context.RiskDetail on rdv.RiskDetailId equals rd.Id
                        join v in _context.Vehicle on rdv.VehicleId equals v.Id
                        join ph in _context.PolicyHistory on rd.Id.ToString() equals ph.PreviousRiskDetailId
                        where v.PreviousVehicleVersionId == id && ph.PolicyStatus == RiskStatus.Active
                        select v;
            return query.Any(x => x.PreviousVehicleVersionId == id);
        }

        public VehiclePremiumRatingFactor GetRatingFactors(Guid vehicleId, Guid riskDetailId)
        {
            return _context.VehiclePremiumRatingFactor.OrderByDescending(x => x.Id).FirstOrDefault(v => v.VehicleId == vehicleId.ToString() && v.RiskDetailId == riskDetailId);
        }

        public Task<bool> UpdateRatingFactors(VehiclePremiumRatingFactor ratingFactors)
        {
            _context.Update(ratingFactors);
            return Task.FromResult(true);
        }

        public async Task<IEnumerable<VehiclePremiumRatingFactor>> GetPremiumRatingFactorsByRiskAsync(Guid riskDetailId, int optionNumber)
        {
            return await _context.VehiclePremiumRatingFactor.Where(x => x.RiskDetailId == riskDetailId && x.OptionId == optionNumber).ToListAsync();
        }

        public async Task DeletePremiumRatingFactorAsync(long id)
        {
            var entity = _context.VehiclePremiumRatingFactor.FirstOrDefault(x => x.Id == id);
            if(entity != null)
            {
                _context.VehiclePremiumRatingFactor.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<Vehicle> GetVersionFromPreviousRiskDetailVersion(Guid? riskDetailId, Guid mainId)
        {
            var riskDetail = await _context.RiskDetail.AsNoTracking().FirstAsync(x => x.Id == riskDetailId);
            var sourceRiskDetail = await _context.RiskDetail.AsNoTracking().FirstOrDefaultAsync(x => x.Id == riskDetail.SourceRiskDetailId);
            if (sourceRiskDetail is null)
                return null;
            var rdv = await _context.RiskDetailVehicle.AsNoTracking()
                                .Include(x => x.Vehicle).AsNoTracking()
                                .FirstOrDefaultAsync(x => x.RiskDetailId == sourceRiskDetail.Id && x.Vehicle.MainId == mainId);
            return rdv?.Vehicle;
        }
    }
}


