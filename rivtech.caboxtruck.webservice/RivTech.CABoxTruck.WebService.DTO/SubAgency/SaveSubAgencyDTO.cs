using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class SaveSubAgencyDTO
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public BusinessTypeDTO BusinessType { get; set; }
        public EntityDTO Entity { get; set; }
    }
}
