using RivTech.CABoxTruck.WebService.DTO.Notes;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.GeneralLiabilityCargo;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.MaintenanceSafety;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class PaymentPortalSearchResponseDTO
    {
        public Guid RiskId { get; set; }
        public NameAndAddressDTO NameAndAddress { get; set; }
        public long? CreatedBy { get; set; }
    }
}
