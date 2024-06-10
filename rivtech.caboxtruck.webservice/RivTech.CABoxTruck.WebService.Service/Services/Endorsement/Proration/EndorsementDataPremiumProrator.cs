using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Vehicles;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using System;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration
{
    public class EndorsementDataPremiumProrator : IEndorsementDataPremiumProrator
    {
        private readonly IPremiumService _premiumService;

        public EndorsementDataPremiumProrator(
            IPremiumService premiumService
        )
        {
            _premiumService = premiumService;
        }

        public async Task ProratePremiumsAsync(IEndorsementData endorsementData)
        {
            var riskEndorsementData = (EndorsementData)endorsementData;
            await _premiumService.UpdateRaterPremiumsAsync(riskEndorsementData.RiskDetailId, true);
        }

    }
}
