using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class BrokerInfo : BaseEntity<Guid>
    {
        public BrokerInfo()
        {
            Id = Guid.NewGuid();
        }

        public Guid? RiskId { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid? AgentId { get; set; }
        public Guid? SubAgencyId { get; set; }
        public Guid? SubAgentId { get; set; }
        public short? YearsWithAgency { get; set; }
        public int? ReasonMoveId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsIncumbentAgency { get; set; }
        public bool? IsBrokeredAccount { get; set; }
        public bool? IsMidtermMove { get; set; }

        //public Risk Risk { get; set; }

        public Agent Agent { get; set; }
        public SubAgent SubAgent { get; set; }

        public Agency Agency { get; set; }
        public SubAgency SubAgency { get; set; }

        public void RenewEffectivityDates(BrokerInfo previousBrokerInfo)
        {
            EffectiveDate = previousBrokerInfo.ExpirationDate;
            ExpirationDate = EffectiveDate?.AddYears(1);
        }
    }
}
