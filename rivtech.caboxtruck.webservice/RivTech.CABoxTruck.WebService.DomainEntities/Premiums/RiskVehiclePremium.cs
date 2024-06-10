using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Premiums
{
    public class RiskVehiclePremium : RiskPremiumBase<VehicleCoveragePremium>
    {
        public Guid VehicleMainId { get; set; }

        public override object Clone()
        {
            return new RiskVehiclePremium
            {
                Id = Guid.Empty,
                RiskDetailId = RiskDetailId,
                VehicleMainId = VehicleMainId,
                CoveragePremium = CoveragePremium,
            };
        }

        public override bool Equals(object obj)
        {
            var other = obj as RiskVehiclePremium;
            return other != null
                && other.RiskDetailId == RiskDetailId
                && other.VehicleMainId == VehicleMainId;
        }


    }
}
