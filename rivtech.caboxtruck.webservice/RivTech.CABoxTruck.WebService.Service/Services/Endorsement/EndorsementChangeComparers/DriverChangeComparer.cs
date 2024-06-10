using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class DriverChangeComparer : IChangeComparer
    {

        public Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var endorsementChangeCollection = new EndorsementChangeCollection();
            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            bool hasChanges = false;

            // Add Driver change item
            var newDrivers = newRiskDTO.Drivers.Where(n => !oldRiskDTO.RiskDetailDrivers.Any(o => o.LicenseNumber == n.LicenseNumber)).ToList();
            if (newDrivers.Any())
            {
                newDrivers.ForEach(x => x.Id = null);
                endorsementChangeCollection.Add(new DriverAddChangeItem
                {
                    Drivers = newDrivers
                });
                hasChanges = true;
            }

            // Updated Driver change item
            var updatedDriver = newRiskDTO.Drivers.Where(n =>
                oldRiskDTO.Drivers.Any(o => o.LicenseNumber == n.LicenseNumber && o.Id != n.Id)
                && newRiskDTO.DeletedDate is null);
            foreach (var newDriver in updatedDriver)
            {
                var oldVeh = oldRiskDTO.Drivers.First(x => x.LicenseNumber == newDriver.LicenseNumber);
                var change = CompareUpdatedDriver(newDriver, oldVeh);
                if (change != null)
                {
                    endorsementChangeCollection.Add(change);
                    hasChanges = true;
                }
            }

            foreach (var driver in newRiskDTO.Drivers)
            {
                var previousDriver = oldRiskDTO.RiskDetailDrivers.FirstOrDefault(x => x.LicenseNumber == driver.LicenseNumber);
                var hasPreviousVehicle = previousDriver != null;
                var isNewVehicleVersionDeleted = driver.DeletedDate != null;
                var isPreviousVehicleVersionDeleted = hasPreviousVehicle && previousDriver.DeletedDate != null;
                bool isJustReinstated = hasPreviousVehicle && !isNewVehicleVersionDeleted && isPreviousVehicleVersionDeleted;
                if (isJustReinstated)
                {
                    endorsementChangeCollection.Add(new DriverReinstateChangeItem
                    {
                        DriverId = driver.PreviousDriverVersionId.Value,
                        LicenseNumber = driver.LicenseNumber,
                        FirstName = driver.FirstName,
                        LastName = driver.LastName
                    });
                    hasChanges = true;
                }
            }


            // Removed driver change item
            //var deletedDrivers = newRiskDTO.Drivers.Where(n =>
            //    oldRiskDTO.Drivers.Any(o => o.LicenseNumber == n.LicenseNumber && o.Id != n.Id)
            //    && (newRiskDTO.DeletedDate != null && oldRiskDTO.DeletedDate is null));
            var deletedDrivers = oldRiskDTO.Drivers.Where(n => !newRiskDTO.Drivers.Any(o => o.LicenseNumber == n.LicenseNumber));
            foreach (var newDriver in deletedDrivers)
            {
                endorsementChangeCollection.Add(new DriverRemoveChangeItem
                {
                    DriverId = newDriver.Id.Value,
                    LicenseNumber = newDriver.LicenseNumber,
                    FirstName = newDriver.FirstName,
                    LastName = newDriver.LastName
                });
                hasChanges = true;
            }

            if (!hasChanges)
                return Task.FromResult<IEnumerable<IEndorsementChangeItem>>(null);

            return Task.FromResult(endorsementChangeCollection.AsEnumerable());
        }

        private IEndorsementChangeItem CompareUpdatedDriver(DriverDTO newDriver, DriverDTO oldDriver)
        {
            var changeItem = new DriverUpdateChangeItem();
            changeItem.LicenseNumber = oldDriver.LicenseNumber;
            if (newDriver.FirstName != oldDriver.FirstName) changeItem.FirstName = newDriver.FirstName;
            if (newDriver.LastName != oldDriver.LastName) changeItem.LastName = newDriver.LastName;
            if (newDriver.MiddleName != oldDriver.MiddleName) changeItem.MiddleName = newDriver.MiddleName;
            if (newDriver.BirthDate != oldDriver.BirthDate) changeItem.BirthDate = newDriver.BirthDate;
            if (newDriver.YrsDrivingExp != oldDriver.YrsDrivingExp) changeItem.YrsDrivingExp = newDriver.YrsDrivingExp;
            if (newDriver.YrsCommDrivingExp != oldDriver.YrsCommDrivingExp) changeItem.YrsCommDrivingExp = newDriver.YrsCommDrivingExp;
            if (newDriver.State != oldDriver.State) changeItem.State = newDriver.State;
            if (newDriver.StateFull != oldDriver.StateFull) changeItem.StateFull = newDriver.StateFull;
            if (newDriver.LicenseNumber != oldDriver.LicenseNumber) changeItem.LicenseNumber = newDriver.LicenseNumber;
            if (newDriver.IsMvr != oldDriver.IsMvr) changeItem.IsMvr = newDriver.IsMvr;
            if (newDriver.YearPoint != oldDriver.YearPoint) changeItem.YearPoint = newDriver.YearPoint;
            if (newDriver.AgeFactor != oldDriver.AgeFactor) changeItem.AgeFactor = newDriver.AgeFactor;
            if (newDriver.TotalFactor != oldDriver.TotalFactor) changeItem.TotalFactor = newDriver.TotalFactor;
            if (newDriver.IsOutOfState != oldDriver.IsOutOfState) changeItem.IsOutOfState = newDriver.IsOutOfState;
            if (newDriver.MvrDate != oldDriver.MvrDate) changeItem.MvrDate = newDriver.MvrDate;
            if (newDriver.HireDate != oldDriver.HireDate) changeItem.HireDate = newDriver.HireDate;
            if (newDriver.isExcludeKO != oldDriver.isExcludeKO) changeItem.isExcludeKO = newDriver.isExcludeKO;
            if (newDriver.koReason != oldDriver.koReason) changeItem.koReason = newDriver.koReason;
            if (newDriver.Options != oldDriver.Options) changeItem.Options = newDriver.Options;
            if (newDriver.IsActive != oldDriver.IsActive) changeItem.IsActive = newDriver.IsActive;
            if (newDriver.DeletedDate != oldDriver.DeletedDate) changeItem.DeletedDate = newDriver.DeletedDate;
            if (newDriver.IsValidated != oldDriver.IsValidated) changeItem.IsValidated = newDriver.IsValidated;
            if (newDriver.IsValidatedKO != oldDriver.IsValidatedKO) changeItem.IsValidatedKO = newDriver.IsValidatedKO;
            if (newDriver.EffectiveDate != oldDriver.EffectiveDate) changeItem.EffectiveDate = newDriver.EffectiveDate;
            if (newDriver.ExpirationDate != oldDriver.ExpirationDate) changeItem.ExpirationDate = newDriver.ExpirationDate;
            /*
    }

    public class DriverIncidentsDTO
    {
        public Guid? Id { get; set; }
        public Guid? DriverId { get; set; }
        public string IncidentType { get; set; }
        public DateTime? IncidentDate { get; set; }
        public DateTime? ConvictionDate { get; set; }
        public Guid? UniqueId { get; set; }
    }


             */

            if (oldDriver.LicenseNumber == newDriver.LicenseNumber && !HasChanges(changeItem))
                return null;

            return changeItem;
        }

        private bool HasChanges(DriverUpdateChangeItem changeItem)
        {
            return changeItem.FirstName != null
                || changeItem.LastName != null
                || changeItem.MiddleName != null
                || changeItem.BirthDate != null
                || changeItem.YrsDrivingExp != null
                || changeItem.YrsCommDrivingExp != null
                || changeItem.State != null
                || changeItem.StateFull != null
                || changeItem.IsMvr != null
                || changeItem.YearPoint != null
                || changeItem.AgeFactor != null
                || changeItem.TotalFactor != null
                || changeItem.IsOutOfState != null
                || changeItem.MvrDate != null
                || changeItem.HireDate != null
                || changeItem.isExcludeKO != null
                || changeItem.koReason != null
                || changeItem.Options != null
                || changeItem.IsActive != null
                || changeItem.DeletedDate != null
                || changeItem.IsValidated != null
                || changeItem.IsValidatedKO != null
                || changeItem.EffectiveDate != null
                || changeItem.ExpirationDate != null;
        }
    }
}
