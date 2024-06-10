using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement
{
    public interface IEndorsementDataPremiumRater
    {
        Task RateAsync(IEndorsementData endorsementData, int endorsementNumber, bool forIssuance = false);
        Task PreloadAsync(IEndorsementData endorsementData);
    }
}
