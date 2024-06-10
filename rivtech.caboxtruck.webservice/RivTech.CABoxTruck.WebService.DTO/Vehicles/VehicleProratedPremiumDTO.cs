using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Vehicles
{
    public class VehicleProratedPremiumDTO
    {
        public Guid VehicleId { get; set; }

        public decimal ALProratedPremium { get; set; } = 0;
        public decimal PDProratedPremium { get; set; } = 0;
        public decimal CargoProratedPremium { get; set; } = 0;
        public decimal CollisionProratedPremium { get; set; } = 0;
        public decimal ComprehensiveProratedPremium { get; set; } = 0;
        public decimal RefrigerationProratedPremium { get; set; } = 0;
    }
}
