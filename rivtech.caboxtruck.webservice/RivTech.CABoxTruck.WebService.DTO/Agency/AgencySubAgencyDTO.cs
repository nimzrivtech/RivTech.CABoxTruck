using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class AgencySubAgencyDTO
    {
        public Guid Id { get; set; }
        public Guid AgencyId { get; set; }
        public Guid SubAgencyId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime AddProcessDate { get; set; }
        public DateTime? RemoveProcessDate { get; set; }
        public AgencyDTO Agency { get; set; }
        public SubAgencyDTO SubAgency { get; set; }
    }
}
