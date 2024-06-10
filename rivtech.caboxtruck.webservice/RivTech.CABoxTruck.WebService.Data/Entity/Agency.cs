using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Agency : BaseEntity<Guid>, ICreatedData, IUpdatedData, IIsActive
    {
        public Agency()
        {
            IsActive = true;
        }

        public Guid EntityId { get; set; }
        public Guid? AgencyId { get; set; } // if has value then this is subageny
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public Entity Entity { get; set; }
    }
}
