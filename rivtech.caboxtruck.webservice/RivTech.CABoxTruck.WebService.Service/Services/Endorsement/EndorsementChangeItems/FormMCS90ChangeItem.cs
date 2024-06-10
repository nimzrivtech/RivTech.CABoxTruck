using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class FormMCS90ChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(FormMCS90ChangeItem);

        public string DisplayText => "";

        public string UsDotNumber { get; set; }
        public DateTime? DateReceived { get; set; }
        public string IssuedTo { get; set; }
        public string State { get; set; }
        public string DatedAt { get; set; }
        public int? PolicyDateDay { get; set; }
        public string PolicyDateMonth { get; set; }
        public string PolicyDateYear { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string InsuranceCompany { get; set; }
        public string CounterSignedBy { get; set; }
        public decimal? AccidentLimit { get; set; }
    }
}
