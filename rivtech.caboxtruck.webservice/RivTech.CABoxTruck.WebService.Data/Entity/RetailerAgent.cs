using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RetailerAgent : BaseEntity<Guid>
    {
        public RetailerAgent()
        {
            CreatedDate = DateTime.Now;
        }

        public Guid EntityId { get; set; }
        public Entity Entity { get; set; }

        public bool IsActive { get; set; }

        public Guid RetailerId { get; set; }
        public Retailer Retailer { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}