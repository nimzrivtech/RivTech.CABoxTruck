using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class ManuscriptModifier : ChangeModifierBase<ManusciptChangeItem>
    {
        private readonly IRiskManuscriptsService _riskManuscriptsService;
        private readonly IMapper _mapper;
        private readonly IRiskManuscriptsRepository _riskManuscriptsRepository;

        public ManuscriptModifier(
            IRiskManuscriptsService riskManuscriptsService,
            IMapper mapper,
            IRiskManuscriptsRepository riskManuscriptsRepository
        ) {
            _riskManuscriptsService = riskManuscriptsService;
            _mapper = mapper;
            _riskManuscriptsRepository = riskManuscriptsRepository;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            ManusciptChangeItem change = CastOriginalType(changeItem);

            if (change.RiskManuscripts != null && change.RiskManuscripts.Count() > 0)
            {
                var existingRiskManuscripts = await _riskManuscriptsRepository.GetAsync(x => x.RiskDetailId == data.RiskDetailId);

                if (existingRiskManuscripts != null && existingRiskManuscripts.Count() > 0)
                {
                    _riskManuscriptsRepository.RemoveRange(existingRiskManuscripts);
                    await _riskManuscriptsRepository.SaveChangesAsync();
                }

                foreach (var riskManuscript in change.RiskManuscripts)
                {
                    riskManuscript.Id = Guid.Empty.ToString();
                    riskManuscript.RiskDetailId = data.RiskDetailId;
                    await _riskManuscriptsService.InsertManuscriptAsync(riskManuscript);
                }
            }
        }
    }
}
