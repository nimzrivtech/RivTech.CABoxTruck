using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Proration
{
    public class VehicleProratedAmount : ProratedAmount
    {
        public RiskVehiclePremiumCollection RiskVehiclePremiums { get; }
        public RiskVehiclePremium SubmissionRiskVehiclePremium { get; }

        public VehicleProratedAmount(
            RiskVehiclePremiumCollection riskVehiclePremiums, 
            AmountInfo info, 
            DateRange policyPeriod, 
            RiskVehiclePremium submissionRiskVehiclePremium
        ) : base(info, policyPeriod)
        {
            RiskVehiclePremiums = riskVehiclePremiums;
            SubmissionRiskVehiclePremium = submissionRiskVehiclePremium;

            UpdatePeriods();
        }

        private void UpdatePeriods()
        {
            foreach (var rvp in RiskVehiclePremiums.Reverse())
            {
                var covPrem = rvp.CoveragePremium;

                decimal annual = Info.Kind switch
                {
                    AmountKind.VehicleAL => covPrem.ALPremium.Annual,
                    //AmountKind.VehiclePD => covPrem.PDPremium.Annual,
                    AmountKind.VehicleCargo => covPrem.CargoPremium.Annual,

                    AmountKind.VehiclePDCollision => covPrem.CollisionPremium.Annual,
                    AmountKind.VehiclePDComprehensive => covPrem.CompFTPremium.Annual,

                    _ => 0m
                };

                var period = new ProratedAmountPeriod(covPrem.Id,
                                annual, 
                                covPrem.CoveragePeriod, 
                                PolicyPeriod,
                                rvp.Equals(SubmissionRiskVehiclePremium));

                _periods.AddLast(period);
            }
        }

    }
}
