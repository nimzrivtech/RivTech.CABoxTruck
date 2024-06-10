using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class VehiclePremiumRepository : BaseRepository<VehiclePremium, int>, IVehiclePremiumRepository
    {
        public VehiclePremiumRepository(AppDbContext context) : base(context)
        {
        }

        public Task<VehiclePremium> GetByRiskDetailVehicleIdAsync(Guid riskDetailId, Guid vehicleId)
        {
            VehiclePremium premiumChange = _context.VehiclePremium
                .Include(x => x.Previous)
                .Where(x => x.RiskDetailId == riskDetailId && x.VehicleId == vehicleId)
                .AsNoTracking()
                .FirstOrDefault();
            return Task.FromResult(premiumChange);
        }


        public async Task<VehiclePremium> GetPrevious(VehiclePremium vehiclePremium)
        {
            var vehicle = _context.Vehicle.First(x => x.Id == vehiclePremium.VehicleId);
            var list = (await GetByMainId(vehicle.MainId)).ToList();

            var premium = list.First(x => x.Id == vehiclePremium.Id);
            var index = list.IndexOf(premium);
            VehiclePremium previousVehPrem = null;
            if(index > 0)
            {
                previousVehPrem = list[index - 1];
            }

            return previousVehPrem;
        }


        public async Task<VehiclePremium> GetPreviousActive(VehiclePremium vehiclePremium)
        {
            var vehicle = _context.Vehicle.AsNoTracking().First(x => x.Id == vehiclePremium.VehicleId);
            var premiums = await GetByMainId(vehicle.MainId);

            IQueryable<VehiclePremium> query = from vehPrem in premiums.AsQueryable()
                                               join ph in _context.PolicyHistory.AsNoTracking() on vehPrem.RiskDetailId.ToString() equals ph.PreviousRiskDetailId
                                               where ph.PolicyStatus == RiskStatus.Active
                                               orderby ph.DateCreated descending
                                               select vehPrem;
            return query.AsNoTracking().FirstOrDefault();
        }

        public Task<IEnumerable<VehiclePremium>> GetByRiskDetail(Guid riskDetailId)
        {
            IEnumerable<VehiclePremium> premiumChanges = _context.VehiclePremium
                .Where(x => x.RiskDetailId == riskDetailId)
                .AsNoTracking();
            return Task.FromResult(premiumChanges);
        }

        public Task<IEnumerable<VehiclePremium>> GetByMainId(Guid mainId, bool? activeRiskDetailOnly = null)
        {
            activeRiskDetailOnly ??= true;

            IQueryable<VehiclePremium> query =
                from vehPrem in _context.VehiclePremium.Include(x => x.Previous).AsNoTracking()
                join veh in _context.Vehicle.AsNoTracking() on vehPrem.VehicleId equals veh.Id
                join rd in _context.RiskDetail.AsNoTracking() on vehPrem.RiskDetailId equals rd.Id
                from ph in _context.PolicyHistory.Where(x => x.PreviousRiskDetailId == rd.Id.ToString()).DefaultIfEmpty()
                where veh.MainId == mainId &&
                    (activeRiskDetailOnly != true || (activeRiskDetailOnly == true && (ph.PolicyStatus == RiskStatus.Active || ph == null)))
                select vehPrem;

            IQueryable<VehiclePremium> orderedList = query.OrderBy(x => x.CreatedDate);

            return Task.FromResult(orderedList.AsEnumerable());
        }

        public async Task<VehiclePremium> GetLatestByMainId(Guid mainId)
        {
            var items = await GetByMainId(mainId);
            return items.OrderBy(x => x.CreatedDate).LastOrDefault();
        }

        public override async Task<VehiclePremium> AddAsync(VehiclePremium vehiclePremium)
        {
            _context.ChangeTracker.Clear();
            if (vehiclePremium.Previous != null)
            {
                var entityEntry = _context.ChangeTracker.Entries<VehiclePremium>().FirstOrDefault(x => x.Entity.Id == vehiclePremium.Previous.Id);
                if (entityEntry != null)
                {
                    entityEntry.State = EntityState.Unchanged;
                } else
                {
                    _context.Entry(vehiclePremium.Previous).State = EntityState.Unchanged;
                }
            }
            return await base.AddAsync(vehiclePremium);
        }

        public new VehiclePremium Update(VehiclePremium vehiclePremium)
        {
            _context.ChangeTracker.Clear();

            //var entries = _context.ChangeTracker.Entries<VehiclePremium>().ToList();
            //EntityEntry<VehiclePremium> entityEntry =  entries.FirstOrDefault(x => x.Entity.Id == vehiclePremium.Id);
            //if (entityEntry != null)
            //{
            //    entityEntry.State = EntityState.Detached;
            //}

            //if (vehiclePremium.Previous != null)
            //{
            //    var previousEntry = entries.FirstOrDefault(x => x.Entity.Id == vehiclePremium.Previous.Id);
            //    if (previousEntry != null)
            //    {
            //        previousEntry.State = EntityState.Detached;
            //    }
            //}

            var entityEntry = _context.Entry(vehiclePremium);

            entityEntry.Reference(x => x.AL).IsModified = true;
            entityEntry.Reference(x => x.PD).IsModified = true;
            entityEntry.Reference(x => x.Cargo).IsModified = true;

            entityEntry.Reference(x => x.CompFT).IsModified = true;
            entityEntry.Reference(x => x.Collision).IsModified = true;
            entityEntry.Reference(x => x.Refrigeration).IsModified = true;

            base.Update(vehiclePremium);

            return entityEntry.Entity;
        }

        public override VehiclePremium Remove(VehiclePremium vehiclePremium)
        {
            var entry = _context.ChangeTracker.Entries<VehiclePremium>().FirstOrDefault(x => x.Entity.Id == vehiclePremium.Id);
            if (entry != null)
            {
                entry.State = EntityState.Deleted;
            }
            return vehiclePremium;
        }
    }
}
