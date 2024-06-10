using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer
{
    public interface  IChangeComparer
    {
        Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj);
    }
}
