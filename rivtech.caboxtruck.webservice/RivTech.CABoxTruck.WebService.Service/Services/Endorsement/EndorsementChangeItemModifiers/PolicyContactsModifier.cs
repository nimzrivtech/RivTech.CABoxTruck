using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class PolicyContactsModifier : ChangeModifierBase<PolicyContactsChangeItem>
    {
        private readonly IRiskPolicyContactService _riskPolicyContactService;

        public PolicyContactsModifier(
            IRiskPolicyContactService riskPolicyContactService
        ) {
            _riskPolicyContactService = riskPolicyContactService;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            var change = CastOriginalType(changeItem);

            var riskPolicyContactDTO = data.RiskDTO.RiskPolicyContacts.FirstOrDefault(x => x.Id == change.Id);
            if (riskPolicyContactDTO is null)
                return;

            riskPolicyContactDTO.TypeId = change.TypeId == riskPolicyContactDTO.TypeId ? riskPolicyContactDTO.TypeId : change.TypeId;
            riskPolicyContactDTO.FirstName = change.FirstName ?? riskPolicyContactDTO.FirstName;
            riskPolicyContactDTO.LastName = change.LastName ?? riskPolicyContactDTO.LastName;
            riskPolicyContactDTO.FullName = change.FullName ?? riskPolicyContactDTO.FullName;
            riskPolicyContactDTO.Email = change.Email ?? riskPolicyContactDTO.Email;
            riskPolicyContactDTO.Phone = change.Phone ?? riskPolicyContactDTO.Phone;
            riskPolicyContactDTO.Address = change.Address ?? riskPolicyContactDTO.Address;
            riskPolicyContactDTO.State = change.State ?? riskPolicyContactDTO.State;
            riskPolicyContactDTO.ZipCode = change.ZipCode ?? riskPolicyContactDTO.ZipCode;
            riskPolicyContactDTO.IsActive = change.IsActive == riskPolicyContactDTO.IsActive ? riskPolicyContactDTO.IsActive : change.IsActive;

            await _riskPolicyContactService.UpdateAsync(riskPolicyContactDTO);
        }
    }
}
