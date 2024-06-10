using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
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
    public class AdditionalInterestModifier : ChangeModifierBase<AdditionalInterestChangeItem>
    {
        private readonly IApplicantAdditionalInterestService _applicantAdditionalInterestService;
        private readonly IMapper _mapper;
        private readonly IRiskAdditionalInterestRepository _riskAdditionalInterestRepository;

        public AdditionalInterestModifier(
            IRiskAdditionalInterestRepository riskAdditionalInterestRepository,
            IApplicantAdditionalInterestService applicantAdditionalInterestService,
            IMapper mapper
        ) {
            _applicantAdditionalInterestService = applicantAdditionalInterestService;
            _riskAdditionalInterestRepository = riskAdditionalInterestRepository;
            _mapper = mapper;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            var change = CastOriginalType(changeItem);

            if (change.AdditionalInterests != null && change.AdditionalInterests.Count() > 0)
            {
                var existingRiskAdditionalInterests = await _riskAdditionalInterestRepository.GetAsync(x => x.RiskDetailId == data.RiskDetailId);

                if (existingRiskAdditionalInterests != null && existingRiskAdditionalInterests.Count() > 0)
                {
                    _riskAdditionalInterestRepository.RemoveRange(existingRiskAdditionalInterests);
                    await _riskAdditionalInterestRepository.SaveChangesAsync();
                }

                foreach (var additionalInterest in change.AdditionalInterests)
                {
                    additionalInterest.Id = Guid.Empty;
                    additionalInterest.RiskDetailId = data.RiskDetailId;
                    await _applicantAdditionalInterestService.InsertAsync(additionalInterest);
                }
            }
        }
    }
}
