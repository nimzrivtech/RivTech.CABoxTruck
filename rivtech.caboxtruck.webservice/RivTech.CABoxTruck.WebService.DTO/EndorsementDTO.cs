using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class EndorsementDTO
    {
    }

    public class EndorsementDetailsDTO
    {
        public int EndorsementNumber { get; set; }
        public string PolicyChanges { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string CancellationReasonId { get; set; }
        public RiskDTO Risk { get; set; }
        public EndorsementPremiumChangeSummaryDTO PremiumChange { get; set; }
        public List<AdditionalInterestChange> AdditionalInterestChanges { get; set; }
    }
}
