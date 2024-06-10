using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Insured : BaseEntity<Guid>, ICreatedData, IUpdatedData, IIsActive
    {
        public Insured()
        {
            CreatedDate = DateTime.Now;
            IsActive = true;
        }
        public Guid EntityId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public Entity Entity { get; set; }
    }
}
