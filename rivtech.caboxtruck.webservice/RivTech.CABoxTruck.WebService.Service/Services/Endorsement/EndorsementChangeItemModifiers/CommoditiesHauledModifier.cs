using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class CommoditiesHauledModifier : ChangeModifierBase<CommoditiesHauledChangeItem>
    {
        private readonly IEntityAddressRepository _entityAddressRepository;
        private readonly IRiskSpecificCommoditiesHauledService _riskSpecificCommoditiesHauledService;
        private readonly IMapper _mapper;

        public CommoditiesHauledModifier(
            IEntityAddressRepository entityAddressRepository,
            IMapper mapper,
            IRiskSpecificCommoditiesHauledService riskSpecificCommoditiesHauledService
        ) {
            _entityAddressRepository = entityAddressRepository;
            _mapper = mapper;
            _riskSpecificCommoditiesHauledService = riskSpecificCommoditiesHauledService;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            CommoditiesHauledChangeItem change = CastOriginalType(changeItem);

            foreach (var commoditiesHauledDto in change.CommoditiesHauled)
            {
                commoditiesHauledDto.Id = System.Guid.Empty;
                commoditiesHauledDto.RiskDetailId = data.RiskDetailId;
                await _riskSpecificCommoditiesHauledService.InsertAsync(commoditiesHauledDto);
            }
        }
    }
}
