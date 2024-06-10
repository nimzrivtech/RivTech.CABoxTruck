using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class BusinessDetailsModifier : ChangeModifierBase<BusinessDetailsChangeItem>
    {
        private readonly IRiskDetailRepository _riskDetailRepository;

        public BusinessDetailsModifier(
            IRiskDetailRepository riskDetailRepository
        ) {
            _riskDetailRepository = riskDetailRepository;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            BusinessDetailsChangeItem change = CastOriginalType(changeItem);

            data.RiskDetail.YearsInBusiness = change.YearsInBusiness ?? data.RiskDetail.YearsInBusiness;

            await _riskDetailRepository.SaveChangesAsync();
        }
    }
}
