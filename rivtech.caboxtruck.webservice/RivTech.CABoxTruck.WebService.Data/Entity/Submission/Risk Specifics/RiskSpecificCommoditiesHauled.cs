using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics
{
    public class RiskSpecificCommoditiesHauled : BaseEntity<Guid>
    {
        public RiskSpecificCommoditiesHauled()
        {
            // CreatedDate = DateTime.Now;
        }

        public string Commodity { get; set; }
        public decimal PercentageOfCarry { get; set; }

        public Guid RiskDetailId { get; set; }
        public RiskDetail RiskDetail { get; set; }

        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public object Clone()
        {
            return (RiskSpecificCommoditiesHauled)MemberwiseClone();
        }
    }
}