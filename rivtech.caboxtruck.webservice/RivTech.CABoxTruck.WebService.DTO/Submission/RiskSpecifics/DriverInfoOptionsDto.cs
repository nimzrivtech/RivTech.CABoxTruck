using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics
{
    public class DriverInfoOptionsDto
    {
        public List<EnumerationDTO> MvrPullingFrequencyOptions { get; set; }
        public List<EnumerationDTO> DriverTrainingOptions { get; set; }
        public List<EnumerationDTO> BackgroundCheckOptions { get; set; }
    }
}
