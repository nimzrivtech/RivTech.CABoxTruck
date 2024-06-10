using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class SubAgent : BaseEntity<Guid>, ICreatedData, IIsActive, IUpdatedData
    {
        public SubAgent()
        {
            this.IsActive = true;
        }

        public Guid EntityId { get; set; }
        public Guid? AgencyId { get; set; } 
        public Guid? SubAgencyId { get; set; } 
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public Entity Entity { get; set; }
    }
}
