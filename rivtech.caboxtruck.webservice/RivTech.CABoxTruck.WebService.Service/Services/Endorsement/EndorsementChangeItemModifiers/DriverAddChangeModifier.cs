using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class DriverAddChangeModifier : ChangeModifierBase<DriverAddChangeItem>
    {
        private readonly IDriverService _driverService;

        public DriverAddChangeModifier(IDriverService driverService)
        {
            _driverService = driverService;
        }

        public async override Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            DriverAddChangeItem change = CastOriginalType(changeItem);

            foreach(var driver in change.Drivers)
            {
                driver.Id = null;
                driver.RiskDetailId = data.RiskDetailId;
                await _driverService.InsertAsync(driver);
            }
        }
    }
}
