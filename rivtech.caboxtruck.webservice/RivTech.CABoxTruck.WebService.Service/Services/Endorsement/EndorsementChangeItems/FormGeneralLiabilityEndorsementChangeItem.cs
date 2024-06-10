using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class FormGeneralLiabilityEndorsementChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(FormGeneralLiabilityEndorsementChangeItem);

        public string DisplayText => "";

        public decimal? InjuryLimit { get; set; }
        public decimal? MedicalExpenses { get; set; }
        public decimal? DamagesRentedToYou { get; set; }
        public decimal? AggregateLimit { get; set; }
    }
}
