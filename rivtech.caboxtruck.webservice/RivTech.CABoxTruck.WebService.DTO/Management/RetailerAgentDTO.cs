using System;
using System.Collections.Generic;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RetailerAgentDTO
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }
        public EntityDTO Entity { get; set; }

        public Guid RetailerId { get; set; }
        public RetailerDTO Retailer { get; set; }

        public bool IsActive { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}