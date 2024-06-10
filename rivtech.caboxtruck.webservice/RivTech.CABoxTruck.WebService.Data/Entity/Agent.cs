using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Agent : BaseEntity<Guid>, ICreatedData, IUpdatedData, IIsActive
    {
        public Agent()
        {
            IsActive = true;
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
