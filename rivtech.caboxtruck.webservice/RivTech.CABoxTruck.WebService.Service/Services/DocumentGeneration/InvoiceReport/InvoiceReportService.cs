using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using RivTech.CABoxTruck.WebService.DTO.Emails.Invoices;
using RivTech.CABoxTruck.WebService.DTO.Enums;
using RivTech.CABoxTruck.WebService.ExternalData.Context;
using RivTech.CABoxTruck.WebService.ExternalData.Entity;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.Emails;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.InvoiceReport
{
    public class InvoiceReportService : IInvoiceReportService
    {
        private readonly IServiceProvider _serviceProvider;
        public IConfiguration Configuration { get; }
        private const string UPLOAD_PATH = "risk-document";
        private readonly IReportService _reportService;
        private readonly IContentService _contentService;
        private readonly IConfiguration _configuration;
        private readonly IFileUploadDocumentService _fileUploadDocumentService;
        private readonly IApplogService _applogService;
        private readonly ILog4NetService _errorLogservice;
        private readonly IRiskService _riskService;
        private readonly HttpClient _client;
        private readonly string _billingBaseUrl;
        private readonly IRiskVehiclePremiumTransactionRepository _vehiclePremiumRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IRiskRepository _riskRepository;
        private readonly IInvoiceEmailService _invoiceEmailService;
        private readonly IDbContextFactory<AppDbExternalContext> _externalDbContextFactory;

        public InvoiceReportService(
            IReportService reportService,
            IContentService contentService,
            IConfiguration configuration,
            IFileUploadDocumentService fileUploadDocumentService,
            IApplogService applogService,
            ILog4NetService errorLogservice,
            IHttpClientFactory factory,
            IRiskService riskService,
            IRiskVehiclePremiumTransactionRepository vehiclePremiumRepository,
            IInvoiceEmailService invoiceEmailService,
            IRiskDetailRepository riskDetailRepository,
            IRiskRepository riskRepository,
            IDbContextFactory<AppDbExternalContext> externalDbContextFactory
        )
        {
            Configuration = configuration;
            _reportService = reportService;
            _contentService = contentService;
            _client = factory.CreateClient("billingClient");
            _configuration = configuration;
            _fileUploadDocumentService = fileUploadDocumentService;
            _applogService = applogService;
            _errorLogservice = errorLogservice;
            _riskService = riskService;
            _vehiclePremiumRepository = vehiclePremiumRepository;
            _riskDetailRepository = riskDetailRepository;
            _riskRepository = riskRepository;
            _invoiceEmailService = invoiceEmailService;
            var billingServiceSettings = Configuration.GetSection("BillingService");
            _billingBaseUrl = billingServiceSettings.GetValue<string>("BaseUrl");
            _externalDbContextFactory = externalDbContextFactory;
        }

        public async Task<List<InvoiceGenerateReportResponse<T>>> GenerateInvoiceReport<T>(List<InvoiceDTO> invoices)
        {
            var reportResponses = new List<InvoiceGenerateReportResponse<T>>();
            foreach (var invoice in invoices)
            {
                //Add check for when Billing data no longer synced to BoxTruck db
                var riskDetailId = await _riskService.GetRiskDetailIdByStatusAsync(invoice.RiskId, RiskDetailStatus.Active);
                if (riskDetailId == Guid.Empty) continue;

                invoice.InvoiceType = InvoiceType.Installment;
                var response = await GenerateInvoiceReport<T>(invoice);
                reportResponses.Add(new InvoiceGenerateReportResponse<T>
                {
                    Invoice = invoice,
                    Output = response.Output,
                    Errors = response.Errors
                });
            }
            return reportResponses;
        }

        public async Task<GenerateReportResponse<T>> GenerateInvoiceReport<T>(InvoiceDTO invoice, bool returnUrl = false, bool fromIssuance = false)
        {
            var reportReponse = new GenerateReportResponse<T>();
            try
            {
                var status = invoice.InvoiceType == InvoiceType.Cancellation ? RiskDetailStatus.Canceled : RiskDetailStatus.Active;
                var riskDetailId = await _riskService.GetRiskDetailIdByStatusAsync(invoice.RiskId, status);
                if (fromIssuance)
                {
                    var risk = await _riskRepository.GetRiskByIdAsync(invoice.RiskId);
                    riskDetailId = risk.CurrentRiskDetailId.Value;
                }

                if (riskDetailId == Guid.Empty) throw new Exception("Risk Detail not found");

                var invoiceType = invoice.InvoiceType;
                var invoicePageData = await _contentService.GetReportData(riskDetailId, invoice.InvoiceType == InvoiceType.Endorsement);

                if (string.IsNullOrEmpty(invoice.InvoiceNumber))
                {
                    var billingInvoices = await CallApi<List<InvoiceDTO>>($"invoice/{invoice.RiskId}");
                    if (billingInvoices == null) throw new Exception("Invoice not found");
                    invoice = invoice.Id != Guid.Empty ? billingInvoices.FirstOrDefault(bi => bi.Id == invoice.Id) : billingInvoices.FirstOrDefault();
                }

                if (invoice == null) throw new Exception("Invoice not found");
                invoice.InvoiceType = invoiceType;

                invoicePageData.Billing = invoice;

                var isFinanced = invoicePageData.Risk?.Binding?.PaymentTypesId == BillingConstants.PaymentTypePremiumFinanced;
                if (isFinanced)
                {
                    var billingSummary = await CallApi<BillingSummaryDTO>($"billing/summary/{invoice.RiskId}");
                    var commission = invoice.BrokerCommission == 0 ? (billingSummary.TotalCommissionBilled ?? 0) : invoice.BrokerCommission;
                    invoicePageData.Billing.BrokerCommission = status == RiskDetailStatus.Canceled ? billingSummary.TotalCommissionBilled ?? 0 : commission;
                }

                if (invoice.InvoiceType == InvoiceType.Cancellation)
                {
                    await SetCancellationData(invoice.RiskId, invoicePageData);
                }

                if (invoice.InvoiceType == InvoiceType.Endorsement)
                {
                    await SetEndorsementData(riskDetailId, invoicePageData);
                }

                var invoiceReportBuilder = ReportBuilder.Init();

                var reportPayLoadBuilder = invoiceReportBuilder.SetUploadPath($"{UPLOAD_PATH}/CA/{DateTime.Now:yyyy-MM}/{invoice.RiskId}/{invoicePageData.Billing.InvoiceNumber}.pdf")
                    .AddPage(PageFactory<InvoicePage>.GetInstance(invoicePageData));

                var reportPayload = reportPayLoadBuilder.Build();

                var document = await SaveToDocument(riskDetailId, reportPayload, invoice);

                // Create agent email content data prior the thread creation of invoice generation.
                var agentEmailContentData = await _invoiceEmailService.CreateContentData(invoice, isFinanced);
                // Generated document in a new thread so the calling api don't need for it
                // to finish and just return immediately..

                var dueToRivington = invoice?.TotalAmountDue ?? 0;
                if (isFinanced) dueToRivington = TotalDueToRiv(invoice);
                if (invoice?.InvoiceType == InvoiceType.Cancellation)
                {
                    dueToRivington = TotalDueToRivCancel(invoice, isFinanced);
                }

                GenerateDocument(reportPayload, document.Id.GetValueOrDefault(), invoice.RiskId, agentEmailContentData, dueToRivington < 0);

                if (returnUrl)
                {
                    var uri = await _reportService.GenerateFileReportLink(reportPayload); //Tester only
                    reportReponse.Output = (T)Convert.ChangeType(uri, typeof(T));
                }
            }
            catch (Exception ex)
            {
                _errorLogservice.Error(ex);
                reportReponse.Errors.Add(ex.Message);
            }

            return reportReponse;
        }

        private async Task SetCancellationData(Guid riskId, ReportData invoicePageData)
        {
            if (invoicePageData.Billing == null) return;

            var billingSummary = await CallApi<BillingSummaryDTO>($"billing/summary/{riskId}");
            var fees = await CallApi<List<TransactionFeeDTO>>($"transaction/fee/{riskId}");

            var newInvoiceDetails = invoicePageData.Billing?.InvoiceDetails?.ToList();

            //Remove, last fees only, not final
            string[] feeTypes = { "Fee", "Transaction Fee" };
            newInvoiceDetails?.RemoveAll(id => feeTypes.Contains(id.AmountSubType.AmountType.Description));

            var feeDetails = fees.GroupBy(f => new { f.Description })
                .Select(f => new InvoiceDetailsDTO
                {
                    AmountSubType = new AmountSubTypeDTO { Description = f.Key.Description, AmountType = new AmountType { Description = "Fee" } },
                    InvoicedAmount = f.Sum(x => x.Amount)
                }).ToList();

            newInvoiceDetails?.AddRange(feeDetails);
            if (newInvoiceDetails != null) invoicePageData.Billing.InvoiceDetails = newInvoiceDetails.ToArray();

            invoicePageData.Billing.FinalPremium = billingSummary.BillingSummaryDetails.Where(s => s.Description.Contains("Premium")).Sum(bs => bs.Written);
            invoicePageData.Billing.FinalTaxes = billingSummary.BillingSummaryDetails.Where(s => s.Description == "Tax").Sum(bs => bs.Written);
            invoicePageData.Billing.FinalFees = billingSummary.BillingSummaryDetails.Where(s => s.Description == "Fee").Sum(bs => bs.Written);
            invoicePageData.Billing.TotalPaid = billingSummary.ActiveTotalPaid;
        }

        private async Task SetEndorsementData(Guid riskDetailid, ReportData reportData)
        {
            var vehiclePremiums = await _vehiclePremiumRepository.GetAsync(reportData.Billing.RiskDetailId);

            var vehiclePremiumChanges = new List<VehiclePremiumChange>();
            foreach (var premium in vehiclePremiums)
            {
                var vehiclePremumChange = new VehiclePremiumChange
                {
                    ChangeType = "ADD",
                    Vin = reportData.DataSets["PolicyVehicles"].FirstOrDefault(v => v.MainId == premium.VehicleMainId)?.VIN,
                    Amount = premium.PremiumChange
                };
                vehiclePremiumChanges.Add(vehiclePremumChange);
            }
            reportData.DataSets["VehiclePremiumChanges"] = vehiclePremiumChanges.Cast<dynamic>().ToList();
        }

        private async Task<FileUploadDocumentDTO> SaveToDocument(Guid riskDetailId, ReportPayload reportPayload, InvoiceDTO invoice)
        {
            var invoiceDescription = invoice?.InvoiceType switch
            {
                InvoiceType.Installment => "Installment",
                InvoiceType.Endorsement => "Endorsement",
                InvoiceType.Cancellation => "Final",
                _ => "Deposit"
            };

            var baseUrl = _configuration.GetSection("Report").GetValue<string>("FileUrl");
            var policypacketdocument = new FileUploadDocumentDTO
            {
                tableRefId = invoice?.Id,
                RiskDetailId = riskDetailId,
                FileCategoryId = 3, // billing
                Description = $"{invoiceDescription} Invoice",
                FileName = $"{invoice?.InvoiceNumber}.pdf",
                FilePath = $"{baseUrl}{reportPayload.Upload}",
                IsUploaded = false,
                IsSystemGenerated = true,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            return await _fileUploadDocumentService.AddUpdateAsync(policypacketdocument);
        }

        /// <summary>
        /// Generates invoice document in a separate thread. Includes email sending of invoice document after generation. 
        /// </summary>
        private void GenerateDocument(ReportPayload reportPayload, Guid documentId, Guid riskId, AgentInvoiceEmailContentData emailContentData, bool isNegativeAmount = false)
        {
            var thread = new Thread(() => 
            {
                 try
                 {
                     var contextFactory = new DbContextFactory();
                     var boxTruckConnectionString = _configuration.GetSection("ConnectionStrings").GetValue<string>("ConCABoxTruck");

                     using var boxTruckContext = contextFactory.Create(boxTruckConnectionString);
                     var result = _reportService.GenerateFileReportLink(reportPayload).Result;
                     if (result == null) throw new Exception("GenerateSaveQuoteReport Unsuccessful");

                     var model = boxTruckContext.FileUploadDocument.FirstOrDefault(x => x.Id == documentId);
                     if (model == null) throw new Exception($"Document {documentId} not found");
                     model.IsUploaded = true;
                     boxTruckContext.FileUploadDocument.Update(model);
                     boxTruckContext.SaveChanges();

                     // Save to FTPDocument for Covenir
                     var source = GenerateDocumentSource.DepositInvoice;

                     if (!string.IsNullOrEmpty(source) && !isNegativeAmount)
                     {
                         boxTruckContext.FTPDocumentTemporary.Add(new FTPDocumentTemporary()
                         {
                             RiskId = riskId,
                             RiskDetailId = model.RiskDetailId,
                             Description = model.Description,
                             FileName = model.FileName,
                             FilePath = model.FilePath,
                             IsUploaded = false,
                             Source = source,
                             BatchId = DateTime.Now.ToString("MMddyyyy"),
                             IsCompiled = false,
                             Category = "MISC",
                             CreatedDate = DateTime.Now
                         });
                         boxTruckContext.FTPDocument.Add(new FTPDocument()
                         {
                             RiskId = riskId,
                             RiskDetailId = model.RiskDetailId,
                             Description = model.Description,
                             FileName = model.FileName,
                             FilePath = model.FilePath,
                             IsUploaded = false,
                             Source = source,
                             BatchId = DateTime.Now.ToString("MMddyyyy"),
                             IsCompiled = false,
                             Category = "MISC",
                             CreatedDate = DateTime.Now
                         });

                        boxTruckContext.SaveChanges();
                    }

                    if (!isNegativeAmount)
                    {
                        // Create email queue with generated email content data for the generated invoice document.
                        emailContentData.AddAttachment(result);
                        using var externalDbContext = _externalDbContextFactory.CreateDbContext();
                        EmailQueue emailQueue = _invoiceEmailService.CreateEmailQueueAsync(emailContentData).GetAwaiter().GetResult();
                        externalDbContext.EmailQueues.Add(emailQueue);
                        externalDbContext.SaveChanges();
                    }
                 }
                 catch (Exception ex)
                 {
                     _errorLogservice.Error(ex);
                 }
             });
            thread.Start();
        }

        private async Task<T> CallApi<T>(string path) where T : class
        {
            using HttpContent content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");
            AddAuthHeaderToken(content);
            var response = await _client.GetAsync($"{_billingBaseUrl}/{path}");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var objFailedResponse = JsonTool.DeserializeObject<BillingFailedResponseDTO>(jsonResponse);
                throw new Exception($"{objFailedResponse.Message}");
            }

            return JsonTool.DeserializeObject<T>(jsonResponse);
        }

        private void AddAuthHeaderToken(HttpContent httpContent)
        {
            _client.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(Data.Common.Services.GetAuthToken()))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Data.Common.Services.GetAuthToken());

            if (Data.Common.Services.GetUserDate().HasValue)
                httpContent.Headers.Add("userdate", Data.Common.Services.GetUserDate()?.ToString("yyyy-MM-ddTHH:mm:ss"));
        }

        private decimal TotalDueToRivCancel(InvoiceDTO billing, bool isFinanced)
        {

            var premium = billing?.FinalPremium ?? 0;
            var taxes = billing?.FinalTaxes ?? 0;
            var fees = billing?.FinalFees ?? 0;

            var total = premium + taxes + fees;

            decimal dueToRivington;
            if (!isFinanced)
            {
                dueToRivington = total - (billing?.TotalPaid ?? 0);
                return dueToRivington;
            }

            var netPolicyCost = total + (billing?.BrokerCommission ?? 0);
            dueToRivington = netPolicyCost - (billing?.TotalPaid ?? 0);

            return dueToRivington;
        }

        private decimal TotalDueToRiv(InvoiceDTO billing)
        {
            var premium = billing?.InvoiceDetails.Where(detail => detail.AmountSubType?.AmountType?.Description == "Premium").Sum(detail => detail.InvoicedAmount) ?? 0;
            var brokerCommission = Math.Abs(billing?.BrokerCommission ?? 0);

            var premiumDue = 0m;
            var totalAmountInvoiced = 0m;
            if (premium >= 0)
            {
                premiumDue = premium - brokerCommission;
                totalAmountInvoiced = (billing?.CurrentAmountInvoiced ?? 0) - brokerCommission;
            }
            else
            {
                premiumDue = premium - -brokerCommission;
                totalAmountInvoiced = (billing?.CurrentAmountInvoiced ?? 0) - -brokerCommission;
            }

            return totalAmountInvoiced;
        }
    }
}