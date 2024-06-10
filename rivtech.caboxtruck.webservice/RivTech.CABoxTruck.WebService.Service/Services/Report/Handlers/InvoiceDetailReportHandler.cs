using AutoMapper;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Common;
using RivTech.CABoxTruck.WebService.Common.ReportTemplates;
using RivTech.CABoxTruck.WebService.DALDapper.IDataAccess;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.Services.Billing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Report.Handlers
{
    public class InvoiceDetailReportHandler
    {
        private readonly IMapper _mapper;
        private readonly IBillingService _billingService;

        public InvoiceDetailReportHandler(IMapper mapper, IBillingService billingService)
    {
        _mapper = mapper;
        _billingService = billingService;
    }

    public async Task<List<InvoiceDetailReportTemplate>> Handler(ReportManagementFilter reportManagementFilter)
        {
            List<InvoiceDetailReportTemplate> invoiceDetailReportTemplates = new List<InvoiceDetailReportTemplate>();

            var invoices = await _billingService.GetInvoicesByDateRange(reportManagementFilter.DateMin.Value, reportManagementFilter.DateMax.Value);

            foreach ( var invoice in invoices)
            {
                foreach ( var invoiceDetail in invoice.InvoiceDetails)
                {
                    InvoiceDetailReportTemplate invoiceDetailReportTemplate = new InvoiceDetailReportTemplate();
                    invoiceDetailReportTemplate.InvoiceNumber = invoice.InvoiceNumber;
                    invoiceDetailReportTemplate.InvoiceDate = invoice.InvoiceDate.ToString("MM/dd/yy");
                    invoiceDetailReportTemplate.DueDate = invoice.DueDate.ToString("MM/dd/yy");
                    invoiceDetailReportTemplate.AmountSubTypeId = invoiceDetail.AmountSubTypeId;
                    invoiceDetailReportTemplate.Description = invoiceDetail.AmountSubType.Description;
                    invoiceDetailReportTemplate.InvoicedAmount = invoiceDetail.InvoicedAmount.ToString();

                    invoiceDetailReportTemplates.Add(invoiceDetailReportTemplate);
                }
            }

            return invoiceDetailReportTemplates;
        }

    }
}
