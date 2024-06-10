using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Retailer : BaseEntity<Guid>
    {
        public Retailer()
        {
            CreatedDate = DateTime.Now;
        }

        public Guid EntityId { get; set; }
        public Entity Entity { get; set; }

        public bool IsActive { get; set; }

        public List<RetailerAgency> LinkedAgencies { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}