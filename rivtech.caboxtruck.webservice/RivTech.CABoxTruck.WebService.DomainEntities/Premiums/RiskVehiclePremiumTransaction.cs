using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Premiums
{
    public class RiskVehiclePremiumTransaction : RiskPremiumTransactionBase
    {
        private RiskVehiclePremiumTransaction() : base() { }

        public RiskVehiclePremiumTransaction(
            Guid riskDetailId, 
            Guid vehicleMainId, 
            RiskVehiclePremiumCollection riskVehiclePremiums, 
            RiskVehiclePremiumTransaction previousTransaction
        ) : base (riskDetailId) {
            VehicleMainId = vehicleMainId;
            RiskVehiclePremiums = riskVehiclePremiums;
            PreviousTransaction = previousTransaction;
        }

        public Guid VehicleMainId { get; private set; }
        public decimal PremiumChangeAL { get; private set; }
        public decimal PremiumChangePD { get; private set; }
        public decimal PremiumChangeColl { get; private set; }
        public decimal PremiumChangeCompFt { get; private set; }

        public decimal PremiumChangeCargo { get; private set; }

        public decimal CurrentALPremium { get; private set; }
        public decimal CurrentPDPremium { get; private set; }
        public decimal CurrentCargoPremium { get; private set; }
        public decimal CurrentCompPremium { get; private set; }
        public decimal CurrentCollPremium { get; private set; }
        public decimal CurrentRefPremium { get; private set; }
        public RiskVehiclePremiumCollection RiskVehiclePremiums { get; }
        public RiskVehiclePremiumTransaction PreviousTransaction { get; }

        private void SetCurrentPremiums()
        {
            CurrentPremium = RiskVehiclePremiums.TotalProratedPremium;
            PreviousPremium = PreviousTransaction?.CurrentPremium ?? 0m;

            CurrentALPremium = RiskVehiclePremiums.TotalProratedALPremium;
            CurrentPDPremium = RiskVehiclePremiums.TotalProratedPDPremium;
            CurrentCargoPremium = RiskVehiclePremiums.TotalProratedCargoPremium;
            CurrentCompPremium = RiskVehiclePremiums.TotalProratedCompPremium;
            CurrentCollPremium = RiskVehiclePremiums.TotalProratedCollPremium;
            CurrentRefPremium = RiskVehiclePremiums.TotalProratedRefPremium;
        }

        public override void CalculatePremiumChange()
        {
            SetCurrentPremiums();
            PremiumChangeAL =  CurrentALPremium - (PreviousTransaction?.CurrentALPremium ?? 0m);
            PremiumChangePD =  CurrentPDPremium - (PreviousTransaction?.CurrentPDPremium ?? 0m);
            PremiumChangeColl = CurrentCollPremium - (PreviousTransaction?.CurrentCollPremium ?? 0m);
            PremiumChangeCompFt = CurrentCompPremium - (PreviousTransaction?.CurrentCompPremium ?? 0m);
            PremiumChangeCargo =  CurrentCargoPremium - (PreviousTransaction?.CurrentCargoPremium ?? 0m);

            base.CalculatePremiumChange();
        }
    }
}
