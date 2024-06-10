using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RetailerAgency : BaseEntity<Guid>
    {
        public RetailerAgency()
        {
            CreatedDate = DateTime.Now;
        }

        public Guid RetailerId { get; set; }
        public Guid AgencyId { get; set; }

        public Retailer Retailer { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}