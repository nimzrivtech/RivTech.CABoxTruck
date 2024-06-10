using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration
{
    public interface IInvoiceReportService
    {
        Task<GenerateReportResponse<T>> GenerateInvoiceReport<T>(InvoiceDTO invoice, bool returnUrl = false, bool fromIssuance = false);

        Task<List<InvoiceGenerateReportResponse<T>>> GenerateInvoiceReport<T>(List<InvoiceDTO> invoices);

    }
}
