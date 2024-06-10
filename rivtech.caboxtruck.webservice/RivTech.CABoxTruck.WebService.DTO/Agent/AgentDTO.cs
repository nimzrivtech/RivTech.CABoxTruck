using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class AgentDTO
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid? SubAgencyId { get; set; }
        public EntityDTO Entity { get; set; }
        public AgencyDTO Agency { get; set; }
        public SubAgencyDTO SubAgency { get; set; }
        public ICollection<AgentSubAgencyDTO> AgentSubAgencies { get; set; }
    }
}