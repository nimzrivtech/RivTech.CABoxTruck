using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Emails.Invoices;
using RivTech.CABoxTruck.WebService.ExternalData.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.IServices.Emails
{
    public interface IInvoiceEmailService
    {
        Task<AgentInvoiceEmailContentData> CreateContentData(InvoiceDTO invoice, bool isFinanced = false);
        Task<EmailQueue> CreateEmailQueueAsync(AgentInvoiceEmailContentData emailContentData);
    }
}
