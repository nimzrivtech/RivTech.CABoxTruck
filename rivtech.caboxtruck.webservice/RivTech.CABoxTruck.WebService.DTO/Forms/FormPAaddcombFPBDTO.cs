using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class FormPAaddcombFPBDTO : IDialogUpdate
    {
        public bool AddedFirstPartyBenefits { get; set; }
        public decimal MedicalExpenseBenefits { get; set; }
        public decimal WorkLossUpTo { get; set; }
        public decimal WorkLossPerMonth { get; set; }
        public decimal FuneralExpenseBenefits { get; set; }
        public decimal AccidentalDeathBenefits { get; set; }
        public bool CombinationFirstPartyBenefits { get; set; }
        public decimal MaximumTotalLimitAllBenefits { get; set; }
        public decimal CombFuneralExpenseBenefits { get; set; }
        public decimal CombAccidentalDeathBenefits { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}