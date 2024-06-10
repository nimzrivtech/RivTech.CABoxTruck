using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.Policy;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.PremiumChanges
{
    public class VehiclePremiumChangeCalculator
    {
        public IEnumerable<EndorsementPendingChange> Calculate(Vehicle newVehicle, Vehicle oldVehicle)
        {
            var list = new List<EndorsementPendingChange>();
            if (newVehicle.Id == oldVehicle.Id)
                return list;

            if (newVehicle.IsDeleted)
            {
                var change = new EndorsementPendingChange
                {
                    Description = newVehicle.VIN,
                    PremiumBeforeEndorsement = oldVehicle.AutoLiabilityPremium,
                    DateCreated = DateTime.Now
                };
            }
            else
            {
                if (newVehicle.AutoLiabilityPremium == oldVehicle.AutoLiabilityPremium)
                {
                    var change = new EndorsementPendingChange
                    {
                        Description = newVehicle.VIN,
                        PremiumBeforeEndorsement = oldVehicle.AutoLiabilityPremium,
                        DateCreated = DateTime.Now
                    };
                }
                else
                {

                }
            }


            return list;
        }

        public IEnumerable<EndorsementPendingChange> CalculateForNewVehicle(Vehicle vehicle)
        {
            var list = new List<EndorsementPendingChange>();

            list.Add(new EndorsementPendingChange
            {
                AmountSubTypeId = BillingConstants.VehicleALP,
                PremiumBeforeEndorsement = 0,
                PremiumAfterEndorsement = vehicle.AutoLiabilityPremium,
                PremiumDifference = vehicle.AutoLiabilityPremium,
                DateCreated = DateTime.Now
            });

            list.Add(new EndorsementPendingChange
            {
                AmountSubTypeId = BillingConstants.VehiclePDP,
                PremiumBeforeEndorsement = 0,
                PremiumAfterEndorsement = vehicle.PhysicalDamagePremium,
                PremiumDifference = vehicle.PhysicalDamagePremium,
                DateCreated = DateTime.Now
            });

            list.Add(new EndorsementPendingChange
            {
                AmountSubTypeId = BillingConstants.VehicleCP,
                PremiumBeforeEndorsement = 0,
                PremiumAfterEndorsement = vehicle.CargoPremium,
                PremiumDifference = vehicle.CargoPremium,
                DateCreated = DateTime.Now
            });

            list.ForEach(x => x.Description = vehicle.VIN);
            return list.Where(x => x.PremiumDifference != 0);
        }

        public IEnumerable<EndorsementPendingChange> CalculateForDeleted(Vehicle vehicle, DateTime effectiveDate)
        {
            var list = new List<EndorsementPendingChange>();
            var proratedPremiumCalculator = new ProratedPremiumCalculator();

            var alChange = new EndorsementPendingChange
            {
                AmountSubTypeId = BillingConstants.VehicleALP,
                PremiumBeforeEndorsement = 0,
                DateCreated = DateTime.Now
            };
            alChange.PremiumAfterEndorsement = proratedPremiumCalculator
                .Calculate(vehicle.AutoLiabilityPremium.Value, vehicle.EffectiveDate.Value, vehicle.ExpirationDate.Value, effectiveDate);
            alChange.PremiumDifference = alChange.PremiumAfterEndorsement - alChange.PremiumBeforeEndorsement;
            list.Add(alChange);

            var pdChange = new EndorsementPendingChange
            {
                AmountSubTypeId = BillingConstants.VehiclePDP,
                PremiumBeforeEndorsement = 0,
                PremiumAfterEndorsement = vehicle.PhysicalDamagePremium,
                PremiumDifference = vehicle.PhysicalDamagePremium,
                DateCreated = DateTime.Now
            };
            list.Add(pdChange);

            var cargoChange = new EndorsementPendingChange
            {
                AmountSubTypeId = BillingConstants.VehicleCP,
                PremiumBeforeEndorsement = 0,
                PremiumAfterEndorsement = vehicle.CargoPremium,
                PremiumDifference = vehicle.CargoPremium,
                DateCreated = DateTime.Now
            };
            list.Add(cargoChange);

            return list.Where(x => x.PremiumDifference != 0);

        }
    }
}
