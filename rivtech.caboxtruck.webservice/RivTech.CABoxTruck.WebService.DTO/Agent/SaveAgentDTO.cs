using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class SaveAgentDTO
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid? SubAgencyId { get; set; }
        public EntityDTO Entity { get; set; }
        public ICollection<AgentSubAgencyDTO> AgentSubAgencies { get; set; }
    }
}