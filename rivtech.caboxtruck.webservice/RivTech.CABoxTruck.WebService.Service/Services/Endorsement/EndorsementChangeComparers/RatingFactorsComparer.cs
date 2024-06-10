using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public class RatingFactorsComparer : IChangeComparer
    {
        public Task<IEnumerable<IEndorsementChangeItem>> Compare(object newRiskDtoObj, object oldRiskDtoObj)
        {
            var newRiskDTO = (RiskDTO)newRiskDtoObj;
            var oldRiskDTO = (RiskDTO)oldRiskDtoObj;

            var newRiskCovPremium = newRiskDTO.RiskCoverages.First(x => x.OptionNumber == newRiskDTO.Binding.BindOptionId);
            var oldRiskCovPremium = oldRiskDTO.RiskCoverages.First(x => x.OptionNumber == oldRiskDTO.Binding.BindOptionId);

            var changeItem = new RatingFactorsChangeItem(); 

            if(AreMatching(newRiskCovPremium.LiabilityScheduleRatingFactor.Value, oldRiskCovPremium.LiabilityScheduleRatingFactor.Value))
            {
                changeItem.ALScheduleRF = newRiskCovPremium.LiabilityScheduleRatingFactor;
            }

            if(AreMatching(newRiskCovPremium.ApdScheduleRF.Value, oldRiskCovPremium.ApdScheduleRF.Value))
            {
                changeItem.PDScheduleRF = newRiskCovPremium.ApdScheduleRF;
            }

            if(AreMatching(newRiskCovPremium.CargoScheduleRF.Value, oldRiskCovPremium.CargoScheduleRF.Value))
            {
                changeItem.CargoScheduleRF = newRiskCovPremium.CargoScheduleRF;
            }

            if(AreMatching(newRiskCovPremium.GlScheduleRF.Value, oldRiskCovPremium.GlScheduleRF.Value))
            {
                changeItem.GLScheduleRF = newRiskCovPremium.GlScheduleRF;
            }

            if (!changeItem.HasChanges())
            {
                changeItem = null;
            }

            return Task.FromResult((new IEndorsementChangeItem[] { changeItem }).AsEnumerable());
        }

        private bool AreMatching(decimal num1, decimal num2)
        {
            return ToFourDecimals(num1) != ToFourDecimals(num2);
        }

        private decimal ToFourDecimals(decimal number)
        {
            return Math.Round(number, 4);
        }
    }
}
