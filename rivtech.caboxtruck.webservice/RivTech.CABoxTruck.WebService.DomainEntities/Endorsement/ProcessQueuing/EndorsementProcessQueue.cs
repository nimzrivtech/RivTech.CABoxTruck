using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.ProcessQueuing
{
    public class EndorsementProcessQueue
    {
        private EndorsementProcessQueue()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid RiskId { get; set; }
        public DateTime QueueTime { get; private set; }
        public DateTime? StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }
        public bool IsArchived { get; private set; } = false;
        public bool IsError { get; private set; } = false;
        public string ProcessType { get; set; }
        public Guid RiskDetailId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string EndorsementText { get; set; }
        public string PolicyChanges { get; set; }

        public bool InProgress => StartTime != null && EndTime is null;


        public void Start()
        {
            StartTime = DateTime.Now;
        }

        public void End()
        {
            if (StartTime is null)
                throw new InvalidOperationException("Unable to end unstarted queue.");
            EndTime = DateTime.Now;
            Archive();
        }

        public void Archive()
        {
            IsArchived = true;
        }

        public static EndorsementProcessQueue Create(EndorsementProcessQueue endorsementProcessQueue)
        {
            return new EndorsementProcessQueue
            {
                RiskId = endorsementProcessQueue.RiskId,
                ProcessType = endorsementProcessQueue.ProcessType,
                EffectiveDate = endorsementProcessQueue.EffectiveDate,
                EndorsementText = endorsementProcessQueue.EndorsementText,
                RiskDetailId = endorsementProcessQueue.RiskDetailId,
                PolicyChanges = endorsementProcessQueue.PolicyChanges,
                QueueTime = DateTime.Now
            };
        }

        public void Fail()
        {
            IsError = true;
        }
    }
}
