using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class VehicleRemoveChangeModifier : ChangeModifierBase<VehicleRemoveChangeItem>
    {
        private readonly IVehicleService _vehicleService;

        public VehicleRemoveChangeModifier(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async override Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            VehicleRemoveChangeItem change = CastOriginalType(changeItem);
            var vehicles = await _vehicleService.GetByRiskDetailIdIncludeAsync(endorsementData.RiskDetailId, true);
            var vehicleToRemove = vehicles.FirstOrDefault(x => x.MainId == change.MainId);
            if (vehicleToRemove is null)
            {
                //throw new OutOfSequenceChangeConflictException($"VIN {change.VIN} does not yet exist on Endorsement {endorsementData.EndorsementNumber} effective {endorsementData.EffectiveDate:MM/dd/yyyy}.");
                throw new OutOfSequenceChangeConflictException($"{change.VIN}", "VehicleRemoveDoesntExist");

            }
            //else if (vehicleToRemove.DeletedDate != null)
            //{
            //    throw new OutOfSequenceChangeConflictException(@$"Failed applying Endorsement {endorsementData.EndorsementNumber} effective {endorsementData.EffectiveDate} 
            //        because vehicle '{change.VIN}' has already been deleted on {vehicleToRemove.ExpirationDate:d}.");
            //}
            vehicleToRemove.ExpirationDate = change.ExpirationDate;
            await _vehicleService.RemoveAsync(vehicleToRemove.Id.Value, true);
            data.RiskDTO.Vehicles = await _vehicleService.GetByRiskDetailIdIncludeAsync(endorsementData.RiskDetailId, true);
        }
    }
}
