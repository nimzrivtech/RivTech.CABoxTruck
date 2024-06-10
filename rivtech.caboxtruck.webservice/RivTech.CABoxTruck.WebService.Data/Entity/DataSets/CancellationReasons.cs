using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity.DataSets
{
    public class CancellationReasons : Enumeration<short>
    {
        public static CancellationReasons CancelFlat = new CancellationReasons(1, "Cancel Flat");
        public static CancellationReasons DriverRequirementsViolation = new CancellationReasons(2, "Driver Requirements Violation");
        public static CancellationReasons FinanceCompanyNonPay = new CancellationReasons(3, "Finance Company Non-Pay");
        public static CancellationReasons IncreasedHazardOrMaterialChange = new CancellationReasons(4, "Increased Hazard or Material Change");
        public static CancellationReasons NonCooperation = new CancellationReasons(5, "Non-Cooperation");
        public static CancellationReasons NonPaymentOfDeductible = new CancellationReasons(6, "Non-Payment of Deductible");
        public static CancellationReasons NonPaymentOfPremium = new CancellationReasons(7, "Non-Payment of Premium");
        public static CancellationReasons SupportingLiabilityPolicyCanceled = new CancellationReasons(8, "Supporting Liability Policy Canceled");
        public static CancellationReasons InsuredRequest = new CancellationReasons(9, "Insured Request");

        public CancellationReasons(short id, string description) : base(id, description) { }

        public CancellationReasons() { }
    }
}
