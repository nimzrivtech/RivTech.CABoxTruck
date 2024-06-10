using System;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RiskAdditionalInterestDTO
    {
        public Guid? Id { get; set; }
        public Guid RiskDetailId { get; set; }
        public short AdditionalInterestTypeId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int EndorsementNumber { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public bool IsWaiverOfSubrogation { get; set; }
        public bool IsPrimaryNonContributory { get; set; }
        public bool IsAutoLiability { get; set; }
        public bool IsGeneralLiability { get; set; }
        public bool Is30DayNotice { get; set; }
        public DateTime? AddProcessDate { get; set; }
        public DateTime? RemoveProcessDate { get; set; }
        public bool IsActive { get; set; }
        public bool fromPreviousEndorsement { get; set; }
    }

    public class AdditionalInterestChange
    {
        public Guid Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public string ChangeType { get; set; }
    }
}