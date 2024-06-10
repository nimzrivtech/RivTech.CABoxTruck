using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class DriverReinstateChangeModifier : ChangeModifierBase<DriverReinstateChangeItem>
    {
        private readonly IDriverService _driverService;

        public DriverReinstateChangeModifier(IDriverService driverService)
        {
            _driverService = driverService;
        }

        public async override Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            DriverReinstateChangeItem change = CastOriginalType(changeItem);


            var drivers = await _driverService.GetByRiskDetailIdIncludeAllAsync(endorsementData.RiskDetailId);
            var driverToReinstate = drivers.FirstOrDefault(x => x.LicenseNumber == change.LicenseNumber);
            if (driverToReinstate is null)
            {
                //var message = @$"Failed applying Endorsement {endorsementData.EndorsementNumber} effective {endorsementData.EffectiveDate:MM/dd/yyyy}
                //    because driver '{change.FirstName} {change.LastName}' doesn't exist.";
                //throw new OutOfSequenceChangeConflictException(message);
                throw new OutOfSequenceChangeConflictException($"{change.FirstName} {change.LastName}", "DriverReinstateDoesntExist");
            }
            else if (driverToReinstate.DeletedDate is null)
            {
                //var message = @$"Failed applying Endorsement {endorsementData.EndorsementNumber} effective {endorsementData.EffectiveDate:MM/dd/yyyy}
                //    because driver '{driverToReinstate.FirstName} {driverToReinstate.LastName}' cannot be reinstated while still active.";
                //throw new OutOfSequenceChangeConflictException(message);
                throw new OutOfSequenceChangeConflictException($"{driverToReinstate.FirstName} {driverToReinstate.LastName}", "DriverReinstateActive");
            }
            await _driverService.ReinstateDriverAsync(driverToReinstate.Id.Value);
            data.RiskDTO.Drivers = await _driverService.GetByRiskDetailIdIncludeAllAsync(endorsementData.RiskDetailId);
        }
    }
}
