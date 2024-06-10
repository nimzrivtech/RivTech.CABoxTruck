using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class SaveAgencyDTO
    {
        public Guid Id { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public EntityDTO Entity { get; set; }
    }
}
