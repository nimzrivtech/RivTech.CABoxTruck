using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class BrokerInfoDTO
    {
        public Guid? Id { get; set; }
        public Guid? RiskId { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid? AgentId { get; set; }
        public Guid? SubAgencyId { get; set; }
        public Guid? SubAgentId { get; set; }
        public string? YearsWithAgency { get; set; }
        public string? ReasonMoveId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsIncumbentAgency { get; set; }
        public bool? IsBrokeredAccount { get; set; }
        public bool? IsMidtermMove { get; set; }
        public string? PolicyNumber { get; set; }

        public AgentDTO Agent { get; set; }
        public AgentSubAgencyDTO SubAgent { get; set; }

        public AgencyDTO Agency { get; set; }
        public SubAgencyDTO SubAgency { get; set; }

        //  public RiskDTO Risk { get; set; }
        public int? AssistantUnderwriterId { get; set; }
        public int? PsrUserId { get; set; }
        public int? TeamLeadUserId { get; set; }
        public int? InternalUWUserId { get; set; }
        public int? ProductUWUserId { get; set; }
    }
}
