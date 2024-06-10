using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics
{
    public class RiskSpecificDestination : BaseEntity<Guid>
    {
        public RiskSpecificDestination()
        {
            this.CreatedDate = DateTime.Now;
        }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public decimal PercentageOfTravelGiven { get; set; }
        public decimal PercentageOfTravelActual { get; set; }
        public Guid RiskDetailId { get; set; }

        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
