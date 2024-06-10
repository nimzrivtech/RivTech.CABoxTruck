using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Endorsements
{
    public class EndorsementProcessStatusLog 
    {
        public static EndorsementProcessStatusLog New(Guid riskId, string initialStatus)
        {
            var log = new EndorsementProcessStatusLog()
            {
                RiskId = riskId,
                DateTime = DateTime.Now,
                Status = initialStatus
            };
            return log;
        }

        private EndorsementProcessStatusLog() { }

        public Guid Id { get; set; }
        public Guid RiskId { get; set; }
        public string Status { get; private set; } 
        public DateTime DateTime { get; private set; }


    }
}
