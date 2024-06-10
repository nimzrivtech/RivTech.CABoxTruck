using System;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.Applicant
{
    public class RiskManuscriptsDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Premium { get; set; }
        public decimal ProratedPremium { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsProrate { get; set; }
        public string PremiumType { get; set; }
        public bool? IsSelected { get; set; }
        public bool? IsActive { get; set; }
        public bool fromPreviousEndorsement { get; set; }

        public Guid RiskDetailId { get; set; }
    }
}