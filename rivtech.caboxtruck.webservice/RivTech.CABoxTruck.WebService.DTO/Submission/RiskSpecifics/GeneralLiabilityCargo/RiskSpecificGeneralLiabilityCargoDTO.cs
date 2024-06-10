using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.GeneralLiabilityCargo
{
    public class RiskSpecificGeneralLiabilityCargoDTO
    {
        public Guid RiskDetailId { get; set; }
        public RiskSpecificGeneralLiabilityDTO GeneralLiability { get; set; }
        public RiskSpecificCargoDTO Cargo { get; set; }
        public List<RiskSpecificCommoditiesHauledDto> CommoditiesHauled { get; set; }
    }
}
