using AutoMapper;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class BusinessNameModifier : ChangeModifierBase<BusinessNameChangeItem>
    {
        private readonly IEntityService _entityService;
        private readonly IEntityRepository _entityRepository;
        private readonly IMapper _mapper;

        public BusinessNameModifier(
            IEntityService entityService,
            IMapper mapper,
            IEntityRepository entityRepository
        ) {
            _entityService = entityService;
            _mapper = mapper;
            _entityRepository = entityRepository;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            BusinessNameChangeItem change = CastOriginalType(changeItem);

            var entity = await _entityRepository.FindAsync(data.RiskDetail.Insured.Entity.Id);

            entity.CompanyName = change.BusinessName ?? data.RiskDetail.Insured.Entity.CompanyName;

            _entityRepository.Update(entity);
            await _entityRepository.SaveChangesAsync();
        }
    }
}
