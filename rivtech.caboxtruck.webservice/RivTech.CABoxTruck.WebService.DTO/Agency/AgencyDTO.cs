using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class AgencyDTO
    {
        public Guid Id { get; set; }

        public Guid AgencyId { get; set; } // if has value then this is subageny

        public Guid EntityId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }        
        public EntityDTO Entity { get; set; }

        public bool IsActive { get; set; }
    }
}


