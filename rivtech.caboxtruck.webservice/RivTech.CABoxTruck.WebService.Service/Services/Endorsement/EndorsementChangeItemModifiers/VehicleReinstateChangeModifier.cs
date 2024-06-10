using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class VehicleReinstateChangeModifier : ChangeModifierBase<VehicleReinstateChangeItem>
    {
        private readonly IVehicleService _vehicleService;

        public VehicleReinstateChangeModifier(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async override Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            VehicleReinstateChangeItem change = CastOriginalType(changeItem);
            var vehicles = await _vehicleService.GetByRiskDetailIdIncludeAsync(endorsementData.RiskDetailId, true);
            var vehicleToReinstate = vehicles.FirstOrDefault(x => x.MainId == change.MainId);
            if (vehicleToReinstate is null)
            {
                //var message = @$"Failed applying Endorsement {endorsementData.EndorsementNumber} effective {endorsementData.EffectiveDate:MM/dd/yyyy}
                //    because VIN '{change.VIN}' doesn't exist.";
                //throw new OutOfSequenceChangeConflictException(message);
                throw new OutOfSequenceChangeConflictException($"{change.VIN}", "VehicleReinstateDoesntExist");
            }
            else if (vehicleToReinstate.DeletedDate is null)
            {
                //var message = @$"Failed applying Endorsement {endorsementData.EndorsementNumber} effective {endorsementData.EffectiveDate:MM/dd/yyyy}
                //    because VIN '{change.VIN}' cannot be reinstated while still active.";
                //throw new OutOfSequenceChangeConflictException(message);
                throw new OutOfSequenceChangeConflictException($"{change.VIN}", "VehicleReinstateActive");
            }
            
            await _vehicleService.ReinstateVehicleAsync(vehicleToReinstate.Id.Value);
            data.RiskDTO.Vehicles = await _vehicleService.GetByRiskDetailIdIncludeAsync(endorsementData.RiskDetailId, true);
        }
    }
}
