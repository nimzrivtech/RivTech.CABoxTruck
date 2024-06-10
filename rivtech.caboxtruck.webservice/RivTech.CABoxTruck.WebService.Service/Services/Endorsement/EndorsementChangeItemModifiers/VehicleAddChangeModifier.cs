using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class VehicleAddChangeModifier : ChangeModifierBase<VehicleAddChangeItem>
    {
        private readonly IVehicleService _vehicleService;

        public VehicleAddChangeModifier(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async override Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            VehicleAddChangeItem change = CastOriginalType(changeItem);
            foreach (var vehicle in change.Vehicles)
            {
                bool isVinExisting = data.RiskDTO.Vehicles.FirstOrDefault(x => x.VIN == vehicle.VIN) != null;
                if (!isVinExisting)
                {
                    vehicle.RiskDetailId = data.RiskDetailId;
                    await _vehicleService.InsertAsync(vehicle);
                }
            }
        }
    }
}
