using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public interface IPacketReportService
    {
        Task<string> GeneratePolicyPacketReport(List<RiskFormDTO> forms);
        Task<string> GenerateSavePolicyPacketReport(Guid riskDetailId, EndorsementDetailsDTO endorsement, string fileName = null);
        Task GenerateEndorsementDocuments(List<EndorsementFormDTO> endorsementForms, InvoiceDTO billing, EndorsementDetailsDTO endorsement, bool sendToEmail = false, bool isFromEndorsement = false, bool isAutoRescind = false);
        Task GenerateNOPCDocuments(List<EndorsementFormDTO> endorsementForms, InvoiceDTO billing, EndorsementDetailsDTO endorsement, bool isAbove100k = false);
    }
}