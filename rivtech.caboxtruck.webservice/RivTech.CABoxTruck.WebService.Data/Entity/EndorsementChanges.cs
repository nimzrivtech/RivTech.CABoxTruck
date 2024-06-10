using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class EndorsementChanges : BaseEntity<Guid>
    {
        public Guid RiskId { get; set; }
        public int EndorsementNumber { get; set; }
        public string Changes { get; set; }
    }
}