using AutoMapper;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class AddressesModifier : ChangeModifierBase<AddressesChangeItem>
    {
        private readonly IEntityAddressRepository _entityAddressRepository;
        private readonly INameAndAddressService _nameAndAddressService;
        private readonly IMapper _mapper;

        public AddressesModifier(
            IEntityAddressRepository entityAddressRepository,
            IMapper mapper,
            INameAndAddressService nameAndAddressService
        ) {
            _entityAddressRepository = entityAddressRepository;
            _mapper = mapper;
            _nameAndAddressService = nameAndAddressService;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            AddressesChangeItem change = CastOriginalType(changeItem);

            //Compares the number of addresses of data and change item
            //If data has higher address count, then do not insert entity address
            //If change has higher address count, then insert entity address
            var isChangeAddressCountHigher = (data.RiskDTO.NameAndAddress.Addresses.Count < change.Addresses.Count);

            foreach (var entityAddressDTO in change.Addresses)
            {
                entityAddressDTO.Address.Id = System.Guid.Empty;
                entityAddressDTO.RiskDetailId = data.RiskDetailId;

                var currentEntityAddressDTO = data.RiskDetail.Insured.Entity.EntityAddresses.FirstOrDefault(x => x.AddressId == entityAddressDTO.AddressId);

                //Insert new entity address if selected entity address does not exist on the current data set
                if (currentEntityAddressDTO == null && isChangeAddressCountHigher)
                {
                    entityAddressDTO.Address.Id = System.Guid.Empty;
                    entityAddressDTO.RiskDetailId = data.RiskDetailId;

                    await _nameAndAddressService.InsertEntityAddressAsync(entityAddressDTO);
                }
            }
        }
    }
}
