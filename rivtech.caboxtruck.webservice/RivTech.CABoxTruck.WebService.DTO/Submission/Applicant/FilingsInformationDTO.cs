using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO.Submission.Applicant
{
    public class FilingsInformationDTO
    {
        public Guid RiskDetailId { get; set; }

        public bool? IsARatedCarrierRequired { get; set; }

        public bool? IsFilingRequired { get; set; }

        public List<int> RequiredFilingIds { get; set; }

        public string USDOTNumber { get; set; }
        public string McNumber { get; set; }

        public bool? IsOthersAllowedToOperateCurrent { get; set; }

        public bool? IsOthersAllowedToOperatePast { get; set; }

        public bool? IsOperatorsWorkOnlyForInsured { get; set; }

        public bool? IsDifferentUSDOTNumberLastFiveYears { get; set; }
        public bool? IsInsuredHasMotorCarrierNo { get; set; }

        public string DOTNumberLastFiveYearsDetail { get; set; }
    }
}