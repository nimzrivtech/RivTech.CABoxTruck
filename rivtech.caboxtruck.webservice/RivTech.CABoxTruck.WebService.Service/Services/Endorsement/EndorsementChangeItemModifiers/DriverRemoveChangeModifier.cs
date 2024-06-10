using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class DriverRemoveChangeModifier : ChangeModifierBase<DriverRemoveChangeItem>
    {
        private readonly IDriverService _driverService;

        public DriverRemoveChangeModifier(IDriverService driverService)
        {
            _driverService = driverService;
        }

        public async override Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            DriverRemoveChangeItem change = CastOriginalType(changeItem);
            

            var drivers = await _driverService.GetByRiskDetailIdIncludeAllAsync(endorsementData.RiskDetailId);
            var driverToRemove = drivers.FirstOrDefault(x => x.LicenseNumber == change.LicenseNumber);
            if (driverToRemove is null)
            {
                //throw new OutOfSequenceChangeConflictException($"Driver '{change.FirstName} {change.LastName}' does not yet exist on Endorsement {endorsementData.EndorsementNumber} effective {endorsementData.EffectiveDate:MM/dd/yyyy}.");
                throw new OutOfSequenceChangeConflictException($"{change.FirstName} {change.LastName}", "DriverRemoveDoesntExist");
            }
            else if (driverToRemove.DeletedDate != null)
            {
                //throw new OutOfSequenceChangeConflictException(@$"Failed applying Endorsement {endorsementData.EndorsementNumber}
                //    because driver '{change.FirstName} {change.LastName}' has already been deleted on.");
                throw new OutOfSequenceChangeConflictException($"{change.FirstName} {change.LastName}", "DriverRemoveDeleted");
            }
            await _driverService.RemoveAsync(driverToRemove.Id.Value, true);
            data.RiskDTO.Drivers = await _driverService.GetByRiskDetailIdIncludeAllAsync(endorsementData.RiskDetailId);
        }
    }
}
