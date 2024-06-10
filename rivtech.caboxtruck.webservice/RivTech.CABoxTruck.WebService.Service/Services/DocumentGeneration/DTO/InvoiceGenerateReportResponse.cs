using NPOI.SS.Formula.Functions;
using RivTech.CABoxTruck.WebService.DTO.Billing;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO
{
    public class InvoiceGenerateReportResponse<T> : GenerateReportResponse<T>
    {
        public InvoiceDTO Invoice { get; set; }
    }
}
