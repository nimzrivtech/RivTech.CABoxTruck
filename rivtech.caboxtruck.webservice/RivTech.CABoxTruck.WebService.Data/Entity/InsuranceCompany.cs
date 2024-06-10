using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class InsuranceCompany : BaseEntity<Guid>, ICreatedData, IUpdatedData
    {
        public Guid EntityId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Entity Entity { get; set; }
    }
}
