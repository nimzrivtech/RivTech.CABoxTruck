using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class VehicleUpdateChangeModifier : ChangeModifierBase<VehicleUpdateChangeItem>
    {
        private readonly IVehicleService _vehicleService;
        private readonly IEndorsementChangesService _endorsementChangesService;

        public VehicleUpdateChangeModifier(
            IVehicleService vehicleService,
            IEndorsementChangesService endorsementChangesService
        )
        {
            _vehicleService = vehicleService;
            _endorsementChangesService = endorsementChangesService;
        }

        public async override Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            VehicleUpdateChangeItem change = CastOriginalType(changeItem);

            var vehicles = data.RiskDTO.Vehicles;
            var vehicle = vehicles.FirstOrDefault(x => x.MainId == change.MainId);
            if (vehicle is null) 
            {
                //throw new OutOfSequenceChangeConflictException(@$"Vehicle {change.VIN} does not yet exist when trying to apply changes of
                //    Endorsement {data.EndorsementNumber} effective {data.EffectiveDate:MM/dd/yyyy}.");
                throw new OutOfSequenceChangeConflictException($"{change.VIN}", "VehicleUpdateDoenstExist");
            }

            if (vehicle.DeletedDate != null)
            {
                return;
                //throw new OutOfSequenceChangeConflictException(@$"Failed applying Endorsement {endorsementData.EndorsementNumber} effective {endorsementData.EffectiveDate:d}
                //    because vehicle '{change.VIN}' cannot be updated when it's already deleted.");
            }

            vehicle.Year = change.Year ?? vehicle.Year;
            vehicle.Make = change.Make ?? vehicle.Make;
            vehicle.Model = change.Model ?? vehicle.Model;
            vehicle.VIN = change.VIN ?? vehicle.VIN;
            vehicle.RegisteredState = change.RegisteredState ?? vehicle.RegisteredState;
            vehicle.Style = change.Style ?? vehicle.Style;
            vehicle.VehicleType = change.VehicleType ?? vehicle.VehicleType;
            vehicle.GrossVehicleWeightId = change.GrossVehicleWeightId ?? vehicle.GrossVehicleWeightId;
            vehicle.VehicleDescriptionId = change.VehicleDescriptionId ?? vehicle.VehicleDescriptionId;
            vehicle.VehicleBusinessClassId = change.VehicleBusinessClassId ?? vehicle.VehicleBusinessClassId;
            vehicle.IsAutoLiability = change.IsAutoLiability ?? vehicle.IsAutoLiability;
            vehicle.IsCoverageFireTheft = change.IsCoverageFireTheft ?? vehicle.IsCoverageFireTheft;
            vehicle.IsCoverageCollision = change.IsCoverageCollision ?? vehicle.IsCoverageCollision;  
            vehicle.CoverageFireTheftDeductibleId = change.CoverageFireTheftDeductibleId ?? vehicle.CoverageFireTheftDeductibleId; 
            vehicle.CoverageCollisionDeductibleId = change.CoverageCollisionDeductibleId ?? vehicle.CoverageCollisionDeductibleId;
            vehicle.StatedAmount = change.StatedAmount ?? vehicle.StatedAmount;
            vehicle.HasCoverageCargo = change.HasCoverageCargo ?? vehicle.HasCoverageCargo;
            vehicle.HasCoverageRefrigeration = change.HasCoverageRefrigeration ?? vehicle.HasCoverageRefrigeration;
            vehicle.UseClassId = change.UseClassId ?? vehicle.UseClassId; 
            vehicle.Radius = change.Radius ?? vehicle.Radius;
            vehicle.GaragingAddressId = change.GaragingAddressId ?? vehicle.GaragingAddressId;
            vehicle.PrimaryOperatingZipCode = change.PrimaryOperatingZipCode ?? vehicle.PrimaryOperatingZipCode;
            vehicle.State = change.State ?? vehicle.State;
            vehicle.RadiusTerritory = change.RadiusTerritory ?? vehicle.RadiusTerritory;

            vehicle.isEndorsement = true;
            data.RiskDTO.Vehicles.Remove(vehicle);
            vehicle = await _vehicleService.UpdateAsync(vehicle);
            data.RiskDTO.Vehicles.Add(vehicle);
        }
    }
}
