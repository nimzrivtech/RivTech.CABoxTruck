using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics
{
    public class RiskSpecificRadiusOfOperation : BaseEntity<Guid>
    {
        public RiskSpecificRadiusOfOperation()
        {
            CreatedDate = DateTime.Now;
        }
        public RiskDetail Risk { get; set; }
        public decimal ZeroToFiftyMilesPercentage { get; set; }
        public decimal FiftyOneToTwoHundredMilesPercentage { get; set; }
        public decimal TwoHundredPlusMilesPercentage { get; set; }
        public decimal OwnerOperatorPercentage { get; set; }
        public bool AnyDestinationToMexicoPlanned { get; set; }
        public bool AnyNyc5BoroughsExposure { get; set; }
        public string ExposureDescription { get; set; }
        public string AverageMilesPerVehicle { get; set; }
        public short? RatingTypeId { get; set; }

        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
