using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class RiskManuscripts : BaseEntity<Guid>
    {
        public RiskManuscripts()
        {
            CreatedDate = DateTime.Now;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Premium { get; set; }
        public decimal ProratedPremium { get; set; }

        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsProrate { get; set; }
        public string PremiumType { get; set; }
        public bool? IsSelected { get; set; }

        public Guid RiskDetailId { get; set; }
        public RiskDetail RiskDetail { get; set; }

        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        [NotMapped]
        public bool IsDeleted => DeletedDate != null;

        [NotMapped]
        public bool IsFlatCancelled
        {
            get
            {
                if (IsDeleted)
                {
                    if (IsProrate)
                    {
                        return false; // prorated manuscripts can be tag as flat cancelled depending on the endorsement effective date in Issuance Page
                    } 
                    else
                    {
                        return ((DeletedDate.Value.Date - EffectiveDate).TotalDays < (ExpirationDate - EffectiveDate).TotalDays);
                    }
                }

                return false;
            }
        }
    }
}