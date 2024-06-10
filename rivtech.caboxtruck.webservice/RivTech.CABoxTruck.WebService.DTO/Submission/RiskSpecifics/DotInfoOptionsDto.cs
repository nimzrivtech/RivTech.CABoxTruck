using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics
{
    public class DotInfoOptionsDto
    {
        public List<EnumerationDTO> ChameleonIssuesOptions { get; set; }
        public List<EnumerationDTO> SaferRatingOptions { get; set; }
        public List<EnumerationDTO> TrafficLightRatingOptions { get; set; }
    }
}
