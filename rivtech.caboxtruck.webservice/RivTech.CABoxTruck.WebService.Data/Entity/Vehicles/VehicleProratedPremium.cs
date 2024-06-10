using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Vehicles
{
    public class VehicleProratedPremium : BaseEntity<Guid>
    {
        public VehicleProratedPremium () { }

        public VehicleProratedPremium(Vehicle vehicle)
        {
            Id = Guid.Empty;
            VehicleId = vehicle.Id;
            ALProratedPremium = vehicle.AutoLiabilityPremium ?? 0;
            PDProratedPremium = vehicle.PhysicalDamagePremium ?? 0;
            CargoProratedPremium = vehicle.CargoPremium ?? 0;
            CollisionProratedPremium = vehicle.CollisionPremium ?? 0;
            ComprehensiveProratedPremium = vehicle.ComprehensivePremium ?? 0;
            RefrigerationProratedPremium = vehicle.RefrigerationPremium ?? 0;
        }

        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public decimal ALProratedPremium { get; set; } = 0;
        public decimal PDProratedPremium { get; set; } = 0;
        public decimal CargoProratedPremium { get; set; } = 0;
        public decimal CollisionProratedPremium { get; set; } = 0;
        public decimal ComprehensiveProratedPremium { get; set; } = 0;
        public decimal RefrigerationProratedPremium { get; set; } = 0;

        public decimal GetTotalProratedPremium()
        {
            var total = ALProratedPremium;
            total += PDProratedPremium;
            total += CargoProratedPremium;
            return total;
        }

        public VehicleProratedPremium Clone()
        {
            var clone = (VehicleProratedPremium)MemberwiseClone();
            clone.Id = Guid.Empty;
            return clone;
        }
    }
}
