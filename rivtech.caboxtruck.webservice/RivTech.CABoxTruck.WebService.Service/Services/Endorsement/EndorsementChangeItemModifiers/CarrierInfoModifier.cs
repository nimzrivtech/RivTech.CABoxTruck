using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class CarrierInfoModifier : ChangeModifierBase<CarrierInfoChangeItem>
    {
        private readonly IBindingRepository _bindingRepository;
        private readonly IMapper _mapper;

        public CarrierInfoModifier(
            IBindingRepository bindingRepository,
            IMapper mapper
        ) {
            _bindingRepository = bindingRepository;
            _mapper = mapper;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            var change = CastOriginalType(changeItem);

            data.RiskDTO.Binding.CreditedOfficeId = change.CreditedOfficeId ?? data.RiskDTO.Binding.CreditedOfficeId;
            data.RiskDTO.Binding.SurplusLIneNum = change.SurplusLIneNum ?? data.RiskDTO.Binding.SurplusLIneNum;

            Binding binding = _mapper.Map<Binding>(data.RiskDTO.Binding);

            _bindingRepository.Update(binding);
            await _bindingRepository.SaveChangesAsync();
        }
    }
}
