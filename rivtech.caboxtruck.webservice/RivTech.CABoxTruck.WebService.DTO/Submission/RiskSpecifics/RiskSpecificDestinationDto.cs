using System;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics
{
    public class RiskSpecificDestinationDto
    {
        public Guid? Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public decimal PercentageOfTravelGiven { get; set; }
        public decimal PercentageOfTravelActual { get; set; }
        public Guid RiskDetailId { get; set; }
    }
}
