using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement
{
    public interface IEndorsementDataCloner
    {
        Task<IEndorsementData> CloneAsync(IEndorsementData endorsementData);
    }
}
