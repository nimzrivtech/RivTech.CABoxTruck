using RivTech.CABoxTruck.WebService.Data.Entity;
using System;

namespace RivTech.CABoxTruck.WebService.DTO.Billing.Fees
{
    public class FeeKind : Enumeration<string>
    {
        public static readonly FeeKind RiskManagementFeeAL = new FeeKind("RMFAL", "Risk Management Fee (AL)");
        public static readonly FeeKind RiskManagementFeePD = new FeeKind("RMFPD", "Risk Management Fee (PD)");
        public static readonly FeeKind InstallmentFee = new FeeKind("INSTF", "Installment Fee");
        public static readonly FeeKind ReinstatementFee = new FeeKind("REINF", "Reinstatement Fee");
        public static readonly FeeKind ReinstatementFee2 = new FeeKind("REINF2", "Reinstatement Fee");

        public static FeeKind FromId(string feeTypeId)
        {
            if (feeTypeId.Equals(RiskManagementFeeAL.Id)) return RiskManagementFeeAL;
            if (feeTypeId.Equals(RiskManagementFeePD.Id)) return RiskManagementFeePD;
            if (feeTypeId.Equals(InstallmentFee.Id)) return InstallmentFee;
            if (feeTypeId.Equals(ReinstatementFee.Id)) return ReinstatementFee;
            if (feeTypeId.Equals(ReinstatementFee2.Id)) return ReinstatementFee2;
            return null;
        }

        private FeeKind(string id, string description) : base(id, description)
        {
        }

    }

    public class FeeDetails
    {
        public static string MultipleALServiceFee = "Service Fee - Multiple Units (AL)";
        public static string SingleALServiceFee = "Service Fee - Single Units (AL)";
        public static string MidtermALServiceFee = "Service Fee - Midterm (AL)";
        public static string MultiplePDServiceFee = "Service Fee - Multiple Units (PD)";
        public static string SinglePDServiceFee = "Service Fee - Single Units (PD)";
        public static string MidtermPDServiceFee = "Service Fee - Midterm (PD)";
        public static string ReinstatementFee = "Reinstatement Fee";
        public static string ReinstatementFee2 = "Reinstatement Fee ";

        public static string FlatMultipleALServiceFee = "Service Fee - Flat Multiple Units (AL)";
        public static string FlatMultiplePDServiceFee = "Service Fee - Flat Multiple Units (PD)";
        public static string FlatMidtermALServiceFee = "Service Fee - Flat Midterm (AL)";
        public static string FlatMidtermPDServiceFee = "Service Fee - Flat Midterm (PD)";

        public string StateCode { get; set; }
        public string FeeKindId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }

        public FeeDetails(string _stateCode, string _feeKindId, string _description, decimal _amount, bool _isActive)
        {
            StateCode = _stateCode;
            FeeKindId = _feeKindId;
            Description = _description;
            Amount = _amount;
            IsActive = _isActive;
        }
    }
}
