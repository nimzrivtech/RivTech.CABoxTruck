using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
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
    public class RiskVehiclePremiumRepository : IRiskVehiclePremiumRepository
    {
        private readonly AppDbContext _dbContext;

        public RiskVehiclePremiumRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync(RiskVehiclePremiumCollection riskVehiclePremiums)
        {
            _dbContext.ChangeTracker.Clear();
            //try
            //{
            foreach (var premium in riskVehiclePremiums)
            {
                var savedItem = _dbContext.RiskVehiclePremium.AsNoTracking()
                    .Include(x => x.CoveragePremium).AsNoTracking()
                    .FirstOrDefault(rvp => rvp.Id == premium.Id);

                if (savedItem is null)
                {
                    if (premium.CoveragePremium.Id != 0)
                    {
                        _dbContext.ChangeTracker.Entries<VehicleCoveragePremium>().ToList().ForEach(x => x.State = EntityState.Detached);
                        _dbContext.Attach(premium.CoveragePremium);
                    }
                    await _dbContext.RiskVehiclePremium.AddAsync(premium);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    _dbContext.ChangeTracker.Entries<RiskVehiclePremium>().ToList().ForEach(x => x.State = EntityState.Detached);
                    _dbContext.ChangeTracker.Entries<VehicleCoveragePremium>().ToList().ForEach(x => x.State = EntityState.Detached);

                    if (premium.CoveragePremium.Id != 0)
                    {
                        _dbContext.Entry(premium.CoveragePremium).State = EntityState.Detached;
                        _dbContext.Attach(premium.CoveragePremium);
                    }
                    _dbContext.RiskVehiclePremium.Update(premium);
                    await _dbContext.SaveChangesAsync();
                }
            }

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.StackTrace);
            //}
        }

        public async Task<IEnumerable<RiskVehiclePremiumCollection>> GetByRiskDetailAsync(Guid riskDetailId, bool fromReinstatement = false)
        {
            var riskDetail = await _dbContext.RiskDetail.AsNoTracking().FirstOrDefaultAsync(x => x.Id == riskDetailId);
            var brokerInfo = await _dbContext.BrokerInfo.AsNoTracking().FirstAsync(x => x.RiskId == riskDetail.RiskId);
            var policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
            var rvpCollList = new List<RiskVehiclePremiumCollection>();

            var rvpItems = await _dbContext.RiskVehiclePremium.AsNoTracking()
                            .Include(x => x.CoveragePremium).AsNoTracking()
                            .Where(rvp => rvp.RiskDetailId == riskDetailId && rvp.FromReinstatement == fromReinstatement)
                            .ToListAsync();

            var groupedItems = rvpItems.GroupBy(x => x.VehicleMainId);

            foreach (var groupedItem in groupedItems)
            {
                var rvpColl = new RiskVehiclePremiumCollection(
                    groupedItem,
                    policyPeriod
                );
                rvpCollList.Add(rvpColl);
            }

            return rvpCollList;
        }

        public async Task<RiskVehiclePremiumCollection> GetByVehicleAsync(Guid riskDetailId, Guid vehicleMainId, bool fromReinstate = false)
        {
            var items = await _dbContext.RiskVehiclePremium
                            .AsNoTracking()
                            .Include(x => x.CoveragePremium)
                            .AsNoTracking()
                            .Where(x => x.VehicleMainId == vehicleMainId && x.RiskDetailId == riskDetailId && x.FromReinstatement == fromReinstate)
                            .ToListAsync();
            if (!items.Any())
            {
                return null;
            }

            var vehicle = _dbContext.Vehicle
                .Include(x => x.RiskDetailVehicles)
                    .ThenInclude(x => x.RiskDetail)
                .FirstOrDefault(x => x.MainId == vehicleMainId);
            var brokerInfo = _dbContext.BrokerInfo.AsNoTracking().FirstOrDefault(x => x.RiskId == vehicle.RiskDetail.RiskId);
            var policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
            items = items.OrderBy(x => x.Order).ToList();
            return new RiskVehiclePremiumCollection(items, policyPeriod);
        }

        public async Task<RiskVehiclePremiumCollection> GetByVehicleByRiskAsync(Guid riskId, Guid riskDetailId, Guid vehicleMainId, bool fromReinstate = false)
        {
            var items = await _dbContext.RiskVehiclePremium
                            .AsNoTracking()
                            .Include(x => x.CoveragePremium)
                            .AsNoTracking()
                            .Where(x => x.VehicleMainId == vehicleMainId && x.RiskDetailId == riskDetailId && x.FromReinstatement == fromReinstate)
                            .ToListAsync();
            if (!items.Any())
            {
                return null;
            }

            var brokerInfo = _dbContext.BrokerInfo.AsNoTracking().FirstOrDefault(x => x.RiskId == riskId);
            var policyPeriod = new DateRange(brokerInfo.EffectiveDate.Value, brokerInfo.ExpirationDate.Value);
            items = items.OrderBy(x => x.Order).ToList();
            return new RiskVehiclePremiumCollection(items, policyPeriod);
        }

        public async Task DeleteAsync(RiskVehiclePremiumCollection currentRiskVehiclePremiums)
        {
            if (currentRiskVehiclePremiums is null)
                return;

            _dbContext.ChangeTracker.Entries<RiskVehiclePremium>().ToList().ForEach(x => x.State = EntityState.Detached);
            _dbContext.ChangeTracker.Entries<VehicleCoveragePremium>().ToList().ForEach(x => x.State = EntityState.Detached);

            foreach (var premium in currentRiskVehiclePremiums)
            {
                var existsInOther = await _dbContext.RiskVehiclePremium
                    .AnyAsync(x => x.CoveragePremium.Id == premium.CoveragePremium.Id
                              && x.RiskDetailId == premium.RiskDetailId);

                _dbContext.RiskVehiclePremium.Remove(premium);
                await _dbContext.SaveChangesAsync();

                if (!existsInOther)
                {
                    _dbContext.RiskVehiclePremiumCoverage.Remove(premium.CoveragePremium);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
