using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BillingConstants
    {
        public const short PaymentTypeFullPay = 1;
        public const short PaymentTypeInstallment = 2;
        public const short PaymentTypePremiumFinanced = 3;

        public const string PayPlanFullPay = "Full";
        public const string PayPlanPremiumFinanced = "PremFin";

        //public const string PaymentMethodCC = "CC";
        //public const string PaymentMethodCheck = "CHQ";
        //public const string PaymentMethodECheck = "EFT";
        //public const string PaymentMethodRecurringCC = "RCC";
        //public const string PaymentMethodRecurringECheck = "RCHQ";
        //public const string PaymentMethodRefund = "R";
        //public const string PaymentMethodWriteOff = "WO";
        //public const string PaymentMethodAdjustment = "ADJ";

        public const string ServiceFeeAL = "ALSF";
        public const string ServiceFeePD = "PDSF";

        public const string InstallmentFee = "IF";

        public const string VehicleALP = "ALP";
        public const string VehicleCP = "CP";
        public const string VehiclePDP = "PDP";
        public const string VehiclePDPColl = "PDPColl";
        public const string VehiclePDPCompFt = "PDPCompFt";

        public const string GeneralLiabilityPremium = "GLP";
        public const string TrailerInterchangePremium = "TIP";
        public const string NonOwnedTrailerPremium = "NOTP";
        public const string AdditionalInsured = "AIP";
        public const string WaiverOfSubrogation = "WS";
        public const string PrimaryAndNoncontributory = "PNC";

        public const string TrailerInterchangeEndst = "TIE";
        public const string HiredPhysicalDamageEndst = "HPDE";

        public const string ALManuscriptPremium = "ALMP";
        public const string PDManuscriptPremium = "PDM";

        public const string SurplusLinesTax = "SLT";
        public const string StampingFee = "SF";

        public const string MinimumPremiumAdjustment = "MPA";
        public const string ShortRatePremium = "SRP";

        public static string GetPremiumMPA(string amountSubType)
        {
            return $"{amountSubType}{MinimumPremiumAdjustment}";
        }

        public static string GetPremiumSRP(string amountSubType)
        {
            return $"{amountSubType}{ShortRatePremium}";
        }

        //public const string RaterTotalPremium = "TotalPre";
        //public const string RaterNHR = "NHR";
        //public const string RaterHUR = "HUR";
        //public const string RaterMGAFee = "MGAFee";
        //public const string RaterEMPATFS = "EMPATFS";
        //public const string RaterPremiumAdj = "PremiumAdj";
        //public const string RaterConsentToRate = "ConsentToRate";
        //public const string RaterCustomFees = "CustomFees";
        //public const string RaterAdditive = "ADDITIVE";

        //public const string RaterInstallmentFee = "Installment Fee";
        //public const string RaterNSFFee = "NSF Fee";

        //public const string RaterTotalPremiumStepAlias = "TotalPremium";
        //public const string RaterMGAFeeStepAlias = "MGAF";
        //public const string RaterEMPATFStepAlias = "EMPATFSur";

        //public const string BillingConsentToRate = "CTR";
        //public const string BillingInstallmentFee = "IF";
        //public const string BillingNSFFee = "NSFF";
        //public const string BillingAdditive = "AD";

        //public const string CreditCardTypeAMEXId = "CCT0";

        //public const string ReversalTypeNSF = "N";

        //public const string RaterPerilValue = "PerilValue";
        //public const string RaterCancellation = "Cancellation";
        //public const string RaterReinstatement = "Reinstatement";
        //public const string RaterEndorsementSameDate = "EndorsementSameDate";
        //public const string RaterEndorsementDiffDate = "EndorsementDiffDate";
    }
}
