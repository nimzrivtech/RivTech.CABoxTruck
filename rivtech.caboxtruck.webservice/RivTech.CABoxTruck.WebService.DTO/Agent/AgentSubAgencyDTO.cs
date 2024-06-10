using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class AgentSubAgencyDTO
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public Guid SubAgencyId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime AddProcessDate { get; set; }
        public string RemoveProcessDate { get; set; }

        public Guid EntityId { get; set; }
        public EntityDTO Entity { get; set; }
    }
}
