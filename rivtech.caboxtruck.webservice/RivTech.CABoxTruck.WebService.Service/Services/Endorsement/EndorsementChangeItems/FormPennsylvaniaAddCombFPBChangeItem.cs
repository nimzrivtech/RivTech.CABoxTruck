using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class FormPennsylvaniaAddCombFPBChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(FormPennsylvaniaAddCombFPBChangeItem);

        public string DisplayText => "";

        public bool? AddedFirstPartyBenefits { get; set; }
        public decimal? MedicalExpenseBenefits { get; set; }
        public decimal? WorkLossUpTo { get; set; }
        public decimal? WorkLossPerMonth { get; set; }
        public decimal? FuneralExpenseBenefits { get; set; }
        public decimal? AccidentalDeathBenefits { get; set; }

        public bool? CombinationFirstPartyBenefits { get; set; }
        public decimal? MaximumTotalLimitAllBenefits { get; set; }
        public decimal? CombFuneralExpenseBenefits { get; set; }
        public decimal? CombAccidentalDeathBenefits { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
