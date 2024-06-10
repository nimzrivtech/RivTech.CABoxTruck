using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class EndorsementPremiumChangeSummaryDTO
    {
        public EndorsementPremiumChangeSummaryDTO()
        {
            this.VehicleChangeList = new List<EndorsementPremiumChangeDTO>();
            this.PolicyChangeList = new List<EndorsementPremiumChangeDTO>();
            this.TotalPremiumChangeList = new List<EndorsementPremiumChangeDTO>();
            this.FeesChangeList = new List<EndorsementPremiumChangeDTO>();
        }
        public List<EndorsementPremiumChangeDTO> VehicleChangeList { get; set; }
        public List<EndorsementPremiumChangeDTO> PolicyChangeList { get; set; }
        public List<EndorsementPremiumChangeDTO> TotalPremiumChangeList { get; set; }
        public List<EndorsementPremiumChangeDTO> FeesChangeList { get; set; }
    }
    public class EndorsementPremiumChangeDTO
    {
        public string Description { get; set; }
        public decimal? CurrentPremium { get; set; }
        public EndorsementPendingChangeDTO EndorsementPendingChange { get; set; }
    }
}
