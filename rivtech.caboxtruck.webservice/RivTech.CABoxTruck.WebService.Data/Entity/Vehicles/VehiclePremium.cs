using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Vehicles
{
    public class VehiclePremium : BaseEntity<int>, ICreatedData
    {

        /// <summary>
        /// For EF use only.
        /// </summary>
        private VehiclePremium()
        {
            SetDefaultVehiclePremiums();
            CreatedDate = DateTime.Now;
        }

        public VehiclePremium(Guid riskDetailId, Guid vehicleId, VehiclePremium previous = null) : this()
        {
            RiskDetailId = riskDetailId;
            VehicleId = vehicleId;
            Previous = previous;
        }

        public Guid RiskDetailId { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal TotalAnnualPremium
        {
            set { }
            get => AL.Annual + PD.Annual + Cargo.Annual;
        }

        public decimal GrossProratedPremium
        {
            set { }
            get => AL.GrossProrated + PD.GrossProrated + Cargo.GrossProrated;
        }

        public decimal NetProratedPremium
        {
            set { }
            get => AL.NetProrated + PD.NetProrated + Cargo.NetProrated;
        }

        public VehiclePremium Previous { get; private set; }

        // Major Premiums
        public VehiclePremiumItem AL { get; private set; }
        public VehiclePremiumItem PD { get; private set; }
        public VehiclePremiumItem Cargo { get; private set; }

        // Sub-premiums
        public VehiclePremiumItem CompFT { get; private set; }
        public VehiclePremiumItem Collision { get; private set; }
        public VehiclePremiumItem Refrigeration { get; private set; }

        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }

        public void ChangeAnnualAL(decimal? annualAL)
        {
            AL = annualAL != null ? AL.ChangeAnnual(annualAL.Value) : VehiclePremiumItem.Empty();
            AL = AL.ChangePrevious(Previous?.AL);
        }

        public void ChangeAnnualPD(decimal? annualPD)
        {
            PD = annualPD != null ? PD.ChangeAnnual(annualPD.Value) : VehiclePremiumItem.Empty();
            PD = PD.ChangePrevious(Previous?.PD);
        }

        public void ChangeAnnualCargo(decimal? annualCargo)
        {
            Cargo = annualCargo != null ? Cargo.ChangeAnnual(annualCargo.Value) : VehiclePremiumItem.Empty();
            Cargo = Cargo.ChangePrevious(Previous?.Cargo);
        }

        public void ChangeAnnualCompFT(decimal? annualCompFT)
        {
            CompFT = annualCompFT != null ? CompFT.ChangeAnnual(annualCompFT.Value) : VehiclePremiumItem.Empty();
            CompFT = CompFT.ChangePrevious(Previous?.CompFT);
        }

        public void ChangeAnnualCollision(decimal? annualCollision)
        {
            Collision = annualCollision != null ? Collision.ChangeAnnual(annualCollision.Value) : VehiclePremiumItem.Empty();
            Collision = Collision.ChangePrevious(Previous?.Collision);
        }

        public void ChangeAnnualRefrigeration(decimal? annualRefrigeration)
        {
            Refrigeration = annualRefrigeration != null ? Refrigeration.ChangeAnnual(annualRefrigeration.Value) : VehiclePremiumItem.Empty();
            Refrigeration = Refrigeration.ChangePrevious(Previous?.Refrigeration);
        }
        public void ChangeAnnualsToZero()
        {
            AL = AL.ChangeAnnual(0);
            PD = PD.ChangeAnnual(0);
            Cargo = Cargo.ChangeAnnual(0);

            Collision = Collision.ChangeAnnual(0);
            CompFT = CompFT.ChangeAnnual(0);
            Refrigeration = Refrigeration.ChangeAnnual(0);
        }

        public void ChangePrevious(VehiclePremium previousPremium)
        {
            Previous = previousPremium;

            AL = AL.ChangePrevious(Previous?.AL);
            PD = PD.ChangePrevious(Previous?.PD);
            Cargo = Cargo.ChangePrevious(Previous?.Cargo);

            CompFT = CompFT.ChangePrevious(Previous?.CompFT);
            Collision = Collision.ChangePrevious(Previous?.Collision);
            Refrigeration = Refrigeration.ChangePrevious(Previous?.Refrigeration);

        }

        private void SetDefaultVehiclePremiums()
        {
            AL = VehiclePremiumItem.Empty();
            PD = VehiclePremiumItem.Empty();
            Cargo = VehiclePremiumItem.Empty();
            CompFT = VehiclePremiumItem.Empty();
            Collision = VehiclePremiumItem.Empty();
            Refrigeration = VehiclePremiumItem.Empty();
        }

        public void Prorate(DateTime effectiveDate, DateTime expiration, DateTime policyEffectiveDate, DateTime policyExpirationDate)
        {
            EffectiveDate = effectiveDate;

            AL = AL.Prorate(effectiveDate, expiration, policyEffectiveDate, policyExpirationDate);
            PD = PD.Prorate(effectiveDate, expiration, policyEffectiveDate, policyExpirationDate);
            Cargo = Cargo.Prorate(effectiveDate, expiration, policyEffectiveDate, policyExpirationDate);
            CompFT = CompFT.Prorate(effectiveDate, expiration, policyEffectiveDate, policyExpirationDate);
            Collision = Collision.Prorate(effectiveDate, expiration, policyEffectiveDate, policyExpirationDate);
            Refrigeration = Refrigeration.Prorate(effectiveDate, expiration, policyEffectiveDate, policyExpirationDate);
        }

        public VehiclePremium Clone()
        {
            var vehiclePremium = new VehiclePremium
            {
                RiskDetailId = Guid.Empty,
                VehicleId = Guid.Empty,
                Previous = this,

                AL = AL.Clone(),
                PD = PD.Clone(),
                Cargo = Cargo.Clone(),

                CompFT = CompFT.Clone(),
                Collision = Collision.Clone(),
                Refrigeration = Refrigeration.Clone(),
            };
            return vehiclePremium;
        }

        public void SetCancelAL(decimal amount)
        {
            AL = AL.SetCancelAmount(amount);
        }

        public void SetCancelPD(decimal amount)
        {
            PD = PD.SetCancelAmount(amount);
        }

        public void SetCancelCargo(decimal amount)
        {
            Cargo = Cargo.SetCancelAmount(amount);
        }

        public void SetCancelCompFT(decimal amount)
        {
            CompFT = CompFT.SetCancelAmount(amount);
        }

        public void SetCancelCollision(decimal amount)
        {
            Collision = Collision.SetCancelAmount(amount);
        }

        public void RecalculateNet()
        {
            AL = AL.RecalculateNet();
            PD = PD.RecalculateNet();
            Cargo = Cargo.RecalculateNet();

            CompFT = CompFT.RecalculateNet();
            Collision = Collision.RecalculateNet();
            Refrigeration = Refrigeration.RecalculateNet();
        }

        public void ChangePreviousGrossAL(decimal grossProrated)
        {
            AL = AL.ChangePreviousGross(grossProrated);
            Previous?.ChangeGrossAL(grossProrated); 
        }

        public void ChangePreviousGrossPD(decimal grossProrated)
        {
            PD = PD.ChangePreviousGross(grossProrated);
            Previous?.ChangeGrossPD(grossProrated); 
        }

        public void ChangePreviousGrossCargo(decimal grossProrated)
        {
            Cargo = Cargo.ChangePreviousGross(grossProrated);
            Previous?.ChangeGrossCargo(grossProrated); 
        }

        public void ChangePreviousGrossCompFT(decimal grossProrated)
        {
            CompFT = CompFT.ChangePreviousGross(grossProrated);
            Previous?.ChangeGrossCompFT(grossProrated); 
        }

        public void ChangePreviousGrossCollision(decimal grossProrated)
        {
            Collision = Collision.ChangePreviousGross(grossProrated);
            Previous?.ChangeGrossCollision(grossProrated); 
        }

        public void ChangePreviousGrossRefrigeration(decimal grossProrated)
        {
            Refrigeration = Refrigeration.ChangePreviousGross(grossProrated);
            Previous?.ChangeGrossRefrigeration(grossProrated); 
        }

        public void ChangeGrossAL(decimal grossProrated)
        {
            AL = AL.ChangeGross(grossProrated);
        }

        public void ChangeGrossPD(decimal grossProrated)
        {
            PD = PD.ChangeGross(grossProrated);
        }
        public void ChangeGrossCargo(decimal grossProrated)
        {
            Cargo = Cargo.ChangeGross(grossProrated);
        }
        public void ChangeGrossCompFT(decimal grossProrated)
        {
            CompFT = CompFT.ChangeGross(grossProrated);
        }
        public void ChangeGrossCollision(decimal grossProrated)
        {
            Collision = Collision.ChangeGross(grossProrated);
        }
        public void ChangeGrossRefrigeration(decimal grossProrated)
        {
            Refrigeration = Refrigeration.ChangeGross(grossProrated);
        }
    }
}
