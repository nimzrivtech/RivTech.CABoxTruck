using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Vehicles
{
    public class VehiclePremiumCoverage
    {
        public static VehiclePremiumCoverage Empty() => new VehiclePremiumCoverage { Annual = 0 };

        public decimal Annual { get; set; }
        public decimal Prorated { get; private set; }

     }
}
