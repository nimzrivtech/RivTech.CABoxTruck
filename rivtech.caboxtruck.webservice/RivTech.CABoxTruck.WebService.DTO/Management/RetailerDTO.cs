using System;
using System.Collections.Generic;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RetailerDTO
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public List<RetailerAgencyDTO> LinkedAgencies { get; set; }

        public EntityDTO Entity { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}