using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class RatingFactorsChangeModifier : ChangeModifierBase<RatingFactorsChangeItem>
    {
        private readonly IRiskCoverageRepository _riskCoverageRepository;

        public RatingFactorsChangeModifier(
            IRiskCoverageRepository riskCoverageRepository
        ) {
            _riskCoverageRepository = riskCoverageRepository;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            var change = CastOriginalType(changeItem);

            var boundRiskCov = await _riskCoverageRepository.FindAsync(x => x.RiskDetailId == data.RiskDTO.Id && x.OptionNumber == data.RiskDTO.Binding.BindOptionId);

            if (boundRiskCov != null)
            {
                boundRiskCov.LiabilityScheduleRatingFactor = change.ALScheduleRF ?? boundRiskCov.LiabilityScheduleRatingFactor;
                boundRiskCov.CargoScheduleRF = change.CargoScheduleRF ?? boundRiskCov.CargoScheduleRF;
                boundRiskCov.ApdScheduleRF = change.PDScheduleRF ?? boundRiskCov.ApdScheduleRF;
                boundRiskCov.GlScheduleRF = change.GLScheduleRF ?? boundRiskCov.GlScheduleRF;

                _riskCoverageRepository.Update(boundRiskCov);
                await _riskCoverageRepository.SaveChangesAsync();
                UpdateEndorsemementData(data, boundRiskCov);
            }
        }

        private void UpdateEndorsemementData(EndorsementData data, RiskCoverage boundRiskCov)
        {
            var dataRiskCov = data.RiskDTO.RiskCoverages.First(x => x.OptionNumber == boundRiskCov.OptionNumber);
            dataRiskCov.LiabilityScheduleRatingFactor = boundRiskCov.LiabilityScheduleRatingFactor;
            dataRiskCov.ApdScheduleRF = boundRiskCov.ApdScheduleRF;
            dataRiskCov.CargoScheduleRF = boundRiskCov.CargoScheduleRF;
            dataRiskCov.GlScheduleRF = boundRiskCov.GlScheduleRF;
        }
    }
}
