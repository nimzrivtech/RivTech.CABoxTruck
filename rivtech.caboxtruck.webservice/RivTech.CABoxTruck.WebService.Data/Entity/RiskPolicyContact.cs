using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RiskPolicyContact : BaseEntity<Guid>, ICreatedData, IUpdatedData
    {
        public RiskPolicyContact()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
        public Guid RiskDetailId { get; set; }
        public Guid EntityId { get; set; }
        public short TypeId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }

        public Entity Entity { get; set; }
    }
}
