using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement
{
    public interface IEndorsementChangeRepository
    {
        Task<IEndorsementChangeCollection> GetByRiskAsync(Guid riskId);
    }
}
