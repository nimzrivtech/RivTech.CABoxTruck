using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class BlanketCoverageDTO
    {
        public Guid RiskDetailId { get; set; }

        public bool? IsAdditionalInsured { get; set; }
        public bool? IsWaiverOfSubrogation { get; set; }
        public bool? IsPrimaryNonContributory { get; set; }
    }
}