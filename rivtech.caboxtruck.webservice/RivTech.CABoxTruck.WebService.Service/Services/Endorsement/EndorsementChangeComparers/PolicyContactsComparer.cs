using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class PolicyContactsComparer : IChangeComparer
    {
        private readonly IRiskPolicyContactService _riskPolicyContactService;
        public PolicyContactsComparer(IRiskPolicyContactService riskPolicyContactService)
        {
            _riskPolicyContactService = riskPolicyContactService;
        }

        public async Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var items = new EndorsementChangeCollection();

            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var newPolicyContacts = newRiskDTO.RiskPolicyContacts;

            foreach (var newPolicyContact in newPolicyContacts)
            {
                var policyContactsChangeItem = new PolicyContactsChangeItem();

                policyContactsChangeItem.TypeId = newPolicyContact.TypeId;
                policyContactsChangeItem.FirstName = newPolicyContact.FirstName;
                policyContactsChangeItem.LastName = newPolicyContact.LastName;
                policyContactsChangeItem.FullName = newPolicyContact.FullName;
                policyContactsChangeItem.Email = newPolicyContact.Email;
                policyContactsChangeItem.Phone = newPolicyContact.Phone;
                policyContactsChangeItem.Address = newPolicyContact.Address;
                policyContactsChangeItem.City = newPolicyContact.City;
                policyContactsChangeItem.State = newPolicyContact.State;
                policyContactsChangeItem.ZipCode = newPolicyContact.ZipCode;
                policyContactsChangeItem.IsActive = newPolicyContact.IsActive;
                policyContactsChangeItem.Id = newPolicyContact.Id;

                items.Add(policyContactsChangeItem);
            }

            return items.Count() > 0 ? items : null;
        }
    }
}
