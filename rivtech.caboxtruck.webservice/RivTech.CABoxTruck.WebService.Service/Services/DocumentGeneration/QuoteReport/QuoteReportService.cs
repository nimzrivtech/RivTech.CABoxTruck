using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Enum;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.Submission;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.QuoteReport
{
    public class QuoteReportService : IQuoteReportService
    {
        private const string UPLOAD_PATH = "risk-document";
        private readonly IReportService _reportService;
        private readonly IContentService _contentService;
        private readonly IRiskService _riskService;
        private readonly IRaterService _raterService;
        private readonly IConfiguration _configuration;
        private readonly IFileUploadDocumentService _fileUploadDocumentService;
        private readonly IApplogService _applogService;
        private readonly ILog4NetService _errorLogservice;
        private readonly IRiskFactory _riskFactory;
        private readonly IRiskDetailRepository _riskDetailRepository;

        public QuoteReportService(IReportService reportService, 
            IContentService contentService,
            IConfiguration configuration,
            IFileUploadDocumentService fileUploadDocumentService,
            IApplogService applogService,
            ILog4NetService errorLogservice,
            IRiskService riskService,
            IRaterService raterService,
            IRiskFactory riskFactory,
            IRiskDetailRepository riskDetailRepository
        ) {
            _reportService = reportService;
            _contentService = contentService;
            _riskService = riskService;
            _raterService = raterService;
            _configuration = configuration;
            _fileUploadDocumentService = fileUploadDocumentService;
            _applogService = applogService;
            _errorLogservice = errorLogservice;
            _riskFactory = riskFactory;
            _riskDetailRepository = riskDetailRepository;
        }

        public async Task<string> GenerateQuoteReport(Guid riskDetailId)
        {
            var riskDto = await _riskService.GetByIdIncludeAsync(riskDetailId);
            var riskDetail = await _riskDetailRepository.FindAsync(riskDetailId);
            var dataSets = await _contentService.GetDataSets(riskDetailId);
            var quotePageData = new ReportData(riskDto) {DataSets = dataSets};
            var quoteReportBuilder = ReportBuilder.Init();

            var reportPayload = quoteReportBuilder
                .SetUploadPath($"{UPLOAD_PATH}/CA/{DateTime.Now:yyyy-MM}/{riskDetailId}/QuoteProposal.pdf")
                .AddPage(PageFactory<QuotePage>.GetInstance(quotePageData));

            var forms = await _riskFactory.GetRiskFormsWithConditionals(quotePageData.Risk, false);
            quotePageData.Risk.RiskForms = forms;

            reportPayload.AddPage(PageFactory<QuoteFormsPage>.GetInstance(quotePageData));
            for (var i = 1; i <= riskDto.RiskCoverages.Count; i++)
            {
                if (riskDto.RiskCoverages[i-1].RiskCoveragePremium?.Premium > 0)
                {
                    reportPayload.AddPage(PageFactory<SchedulesPage>
                        .GetInstance(new ReportData(riskDto) {Id = i.ToString(), DataSets = dataSets}));
                }
            }

            reportPayload.AddPage(PageFactory<QuoteVerification>.GetInstance(quotePageData));
            reportPayload.AddPage(PageFactory<LastPage>.GetInstance(null));

            if(riskDto.NameAndAddress?.State == "CA")
            {
                reportPayload.AddPage(PageFactory<CAQuoteD1>.GetInstance(null));
                reportPayload.AddPage(PageFactory<CAQuoteSL2>.GetInstance(quotePageData));
                reportPayload.AddPage(PageFactory<CAQuoteUM>.GetInstance(quotePageData));
            }

            string result;
            try
            {
                var payload = reportPayload.Build();
                result = await _reportService.GenerateFileReportLink(payload);
                await _applogService.LogDocumentGeneration($"Risk:{riskDetailId}", JsonConvert.SerializeObject(payload), result);
            }
            catch
            {
                await _applogService.LogDocumentGeneration($"Risk:{riskDetailId}", JsonConvert.SerializeObject(reportPayload), "GenerateSaveQuoteReport Unsuccessful");
                throw;
            }

            if (!riskDto.IsRaterVersionLocked)
            {
                riskDetail.RequestedQuoteDate = DateTime.Now;
                await _riskDetailRepository.SaveChangesAsync();
            }

            await _raterService.LockRaterVersionAsync(riskDto.RiskId);
            
            return result;
        }

        public async Task<string> GenerateSaveQuoteReport(Guid newRiskDetailId)
        {
            var riskDto = await _riskService.GetByIdIncludeAsync(newRiskDetailId);
            var dataSets = await _contentService.GetDataSets(newRiskDetailId);
            var quotePageData = new ReportData(riskDto) { DataSets = dataSets };

            var quoteReportBuilder = ReportBuilder.Init();

            var reportPayloadBuilder = quoteReportBuilder
                .SetUploadPath($"{UPLOAD_PATH}/CA/{DateTime.Now:yyyy-MM}/{newRiskDetailId}/QuoteProposal.pdf")
                .AddPage(PageFactory<QuotePage>.GetInstance(quotePageData));

            var forms = await _riskFactory.GetRiskFormsWithConditionals(quotePageData.Risk, false);
            quotePageData.Risk.RiskForms = forms;

            reportPayloadBuilder.AddPage(PageFactory<QuoteFormsPage>.GetInstance(quotePageData));
            for (var i = 1; i <= riskDto.RiskCoverages.Count; i++)
            {
                if (riskDto.RiskCoverages[i - 1].RiskCoveragePremium?.Premium > 0)
                {
                    reportPayloadBuilder.AddPage(PageFactory<SchedulesPage>
                        .GetInstance(new ReportData(riskDto) { Id = i.ToString(), DataSets = dataSets }));
                }
            }

            reportPayloadBuilder.AddPage(PageFactory<QuoteVerification>.GetInstance(quotePageData));
            reportPayloadBuilder.AddPage(PageFactory<LastPage>.GetInstance(null));

            if (riskDto.NameAndAddress?.State == "CA")
            {
                reportPayloadBuilder.AddPage(PageFactory<CAQuoteD1>.GetInstance(null));
                reportPayloadBuilder.AddPage(PageFactory<CAQuoteSL2>.GetInstance(quotePageData));
                reportPayloadBuilder.AddPage(PageFactory<CAQuoteUM>.GetInstance(quotePageData));
            }
            
            var reportPayload = reportPayloadBuilder.Build();

            #region SaveFileUploadDocument
            // Save to FileUploadDocument
            var reportServiceSection = _configuration.GetSection("Report");
            var baseUrl = reportServiceSection.GetValue<string>("FileUrl");
            var policypacketdocument = new FileUploadDocumentDTO()
            {
                tableRefId = newRiskDetailId,
                RiskDetailId = newRiskDetailId,
                FileCategoryId = 1, // Miscellaneous
                Description = "Quote Proposal",
                FileName = "QuoteProposal.pdf",
                FilePath = $"{baseUrl}{reportPayload.Upload}",
                IsUploaded = false,
                IsSystemGenerated = true,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            var fileresult = await _fileUploadDocumentService.AddUpdateAsync(policypacketdocument);
            #endregion

            // Generate Policy Packet Thread
            Thread thread = new Thread(() =>
            {
                try
                {
                    var contextFactory = new DbContextFactory();
                    var connectionString =
                        _configuration.GetSection("ConnectionStrings").GetValue<string>("ConCABoxTruck");
                    using var context = contextFactory.Create(connectionString);
                    var result = _reportService.GenerateFileReportLink(reportPayload).Result;
                    if (result == null)
                        throw new Exception("GenerateSaveQuoteReport Unsuccessful");

                    // Update FileUpload IsUploaded to true
                    var res = context.FileUploadDocument.Where(x => x.Id == fileresult.Id).FirstOrDefault();
                    _applogService.LogDocumentGeneration($"Quote:{newRiskDetailId}", JsonConvert.SerializeObject(reportPayload), result);
                    res.IsUploaded = true;
                    context.FileUploadDocument.Update(res);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _errorLogservice.Error(ex);
                    _applogService.LogDocumentGeneration($"Quote:{newRiskDetailId}", JsonConvert.SerializeObject(reportPayload), "GenerateSaveQuoteReport Unsuccessful");
                }
            });

            thread.Start();

            return reportPayload.Upload;
        }
    }
}