using System.Collections.Generic;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO
{
    public class ReportData
    {
        public ReportData(RiskDTO risk)
        {
            Risk = risk;
        }

        public RiskDTO Risk { get; }

        public EndorsementDetailsDTO Endorsement { get; set; }

        public InvoiceDTO Billing { get; set; }

        public Dictionary<string, List<dynamic>> DataSets { get; set; }
        public RiskAdditionalInterestDTO AIWithNotice { get; set; }
        public string Id { get; set; }
    }
}
