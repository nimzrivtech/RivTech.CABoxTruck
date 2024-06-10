using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RiskAdditionalInterest : BaseEntity<Guid>
    {
        public RiskAdditionalInterest()
        {
            // this.EffectiveDate = DateTime.Now;
            ExpirationDate = DateTime.Now.AddYears(5);
            AddProcessDate = DateTime.Now;
        }
        public Guid RiskDetailId { get; set; }
        public Guid EntityId { get; set; }
        public short AdditionalInterestTypeId { get; set; }

        public string AdditionalInterestType;
        public string FullName;
        public string StreetAddress1;
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime AddProcessDate { get; set; }
        public long AddedBy { get; set; }
        public DateTime? RemoveProcessDate { get; set; }
        public long? RemovedBy { get; set; }
        public int EndorsementNumber { get; set; }
        public bool IsWaiverOfSubrogation { get; set; }
        public bool IsPrimaryNonContributory { get; set; }
        public bool IsAutoLiability { get; set; }
        public bool IsGeneralLiability { get; set; }
        public bool Is30DayNotice { get; set; }
        public Entity Entity { get; set; }
    }
}
