using System;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics
{
    public class RiskSpecificRadiusOfOperationDto
    {
        public Guid? Id { get; set; }
        public decimal ZeroToFiftyMilesPercentage { get; set; }
        public decimal FiftyOneToTwoHundredMilesPercentage { get; set; }
        public decimal TwoHundredPlusMilesPercentage { get; set; }
        public decimal OwnerOperatorPercentage { get; set; }
        public bool AnyDestinationToMexicoPlanned { get; set; }
        public bool AnyNyc5BoroughsExposure { get; set; }
        public string ExposureDescription { get; set; }
        public string AverageMilesPerVehicle { get; set; }
        public short? RatingTypeId { get; set; }
    }
}
