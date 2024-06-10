using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.DTO.Enum;
using RivTech.CABoxTruck.WebService.Service.Services.Submission;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.Services.Emails;
using RivTech.CABoxTruck.WebService.DTO.Emails;
using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.ExternalData.Context;
using RivTech.CABoxTruck.WebService.Service.IServices.Emails;
using RivTech.CABoxTruck.WebService.ExternalData.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class PacketReportService : IPacketReportService
    {
        private const string UPLOAD_PATH = "risk-document";
        private readonly IReportService _reportService;
        private readonly IContentService _contentService;
        private readonly IFileUploadDocumentService _fileUploadDocumentService;
        private readonly IApplogService _applogService;
        private readonly ILog4NetService _errorLogservice;
        private readonly IConfiguration _configuration;
        private readonly IFormsService _formsService;
        private readonly IRiskFactory _riskFactory;
        private readonly IFTPClientServerSevice _FTPClientServerSevice;
        private readonly IDbContextFactory<AppDbExternalContext> _externalDbContextFactory;
        private readonly INOPCEmailService _NOPCEmailService;
        private readonly IEndorsementEmailService _endorsementEmailService;
        private readonly IRiskRepository _riskRepository;
        private readonly IApplicantAdditionalInterestService _additionalInterestService;

        public PacketReportService(IReportService reportService,
            IContentService contentService,
            IFileUploadDocumentService fileUploadDocumentService,
            IApplogService applogService,
            ILog4NetService errorLogservice,
            IConfiguration configuration,
            IFormsService formsService,
            IRiskFactory riskFactory,
            IFTPClientServerSevice FTPClientServerSevice,
            IDbContextFactory<AppDbExternalContext> externalDbContextFactory,
            INOPCEmailService NOPCEmailService,
            IEndorsementEmailService endorsementEmailService,
            IRiskRepository riskRepository,
            IApplicantAdditionalInterestService additionalInterestService
            )
        {
            _reportService = reportService;
            _contentService = contentService;
            _fileUploadDocumentService = fileUploadDocumentService;
            _applogService = applogService;
            _errorLogservice = errorLogservice;
            _configuration = configuration;
            _formsService = formsService;
            _riskFactory = riskFactory;
            _FTPClientServerSevice = FTPClientServerSevice;
            _externalDbContextFactory = externalDbContextFactory;
            _NOPCEmailService = NOPCEmailService;
            _endorsementEmailService = endorsementEmailService;
            _riskRepository = riskRepository;
            _additionalInterestService = additionalInterestService;
        }

        public async Task<string> GeneratePolicyPacketReport(List<RiskFormDTO> forms)
        {
            var riskDetailId = forms[0].RiskDetailId;
            var reportData = await _contentService.GetReportData(riskDetailId);
            forms = await _riskFactory.GetRiskFormsWithConditionals(reportData.Risk, riskForms: forms);
            reportData.Risk.RiskForms = forms;

            var packetFactory = new PolicyPacketPageFactory(riskDetailId, reportData, GetAttachedPages(forms));
            var formsIncluded = packetFactory.Factories.Values.Where(x => x.isAttached).ToList();

            var fileName = forms.Count == 1
                ? forms[0].Form?.IsSupplementalDocument == true ? RemoveInvalidChars(forms[0].Form?.Name) : forms[0].Form?.FormNumber
                : "PolicyPacket";
            var packetReportBuilder = ReportBuilder.Init();
            var reportPayload = packetReportBuilder.SetUploadPath($"{UPLOAD_PATH}/CA/{DateTime.Now:yyyy-MM}/{riskDetailId}/{fileName}.pdf")
            .AddPage(formsIncluded)
            .Build();

            var result = await _reportService.GenerateFileReportLink(reportPayload);

            await _applogService.LogDocumentGeneration($"RiskDetailId:{riskDetailId}", JsonConvert.SerializeObject(reportPayload), result ?? "Generate file unsuccessful");
            return result;
        }

        public async Task<string> GenerateSavePolicyPacketReport(Guid riskDetailId, EndorsementDetailsDTO endorsement, string fileName = null)
        {
            var reportData = await _contentService.GetReportData(riskDetailId, true);
            var isEndorsement = fileName != null;
            var forms = await _riskFactory.GetRiskFormsWithConditionals(reportData.Risk, isEndorsement);
            reportData.Risk.RiskForms = forms;
            reportData.Endorsement = endorsement;

            var packetFactory = new PolicyPacketPageFactory(riskDetailId, reportData, GetAttachedPages(forms));
            var formsIncluded = packetFactory.Factories.Values.Where(x => x.isAttached).ToList();

            fileName ??= forms.Count == 1 ? forms[0].Form?.FormNumber : "PolicyPacket";

            var packetReportBuilder = ReportBuilder.Init();
            var reportPayload = packetReportBuilder.SetUploadPath($"{UPLOAD_PATH}/CA/{DateTime.Now:yyyy-MM}/{riskDetailId}/{fileName}.pdf")
                .AddPage(formsIncluded)
                .Build();

            #region SaveFileUploadDocument
            // Save to FileUploadDocument
            var reportServiceSection = _configuration.GetSection("Report");
            var baseUrl = reportServiceSection.GetValue<string>("FileUrl");
            var descPrefix = isEndorsement ? $"Endorsement {endorsement.EndorsementNumber} " : "";
            var policypacketdocument = new FileUploadDocumentDTO()
            {
                tableRefId = riskDetailId,
                RiskDetailId = riskDetailId,
                FileCategoryId = 1, // Miscellaneous
                Description = $"{descPrefix}Policy Packet - {(endorsement.EffectiveDate == DateTime.MinValue ? DateTime.Now : endorsement.EffectiveDate):MM/dd/yyyy}", //Changed from DateTime.Now to Endorsement Effective Date
                FileName = $"{fileName}.pdf",
                FilePath = $"{baseUrl}{reportPayload.Upload}",
                IsUploaded = false,
                IsSystemGenerated = true,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            var fileresult = await _fileUploadDocumentService.AddUpdateAsync(policypacketdocument);
            #endregion

            GenerateFile(new[] { new { fileresult.Id, Payload = reportPayload } });

            return reportPayload.Upload;
        }

        public async Task GenerateEndorsementDocuments(List<EndorsementFormDTO> endorsementForms, InvoiceDTO billing, EndorsementDetailsDTO endorsement, bool sendToEmail = false, bool isFromEndorsement = false, bool isAutoRescind = false)
        {
            //var riskDetailId = endorsement.Risk.Id.GetValueOrDefault();
            var risk = await _riskRepository.GetRiskByIdAsync(endorsement.Risk.RiskId);
            var riskDetailId = risk.CurrentRiskDetailId.Value;

            var endorsementDocuments = new List<dynamic>();
            var reportData = await _contentService.GetReportData(riskDetailId, true);
            reportData.Risk.RiskForms = await _riskFactory.GetRiskFormsWithConditionals(reportData.Risk, true);
            reportData.Risk.AdditionalInterest = (await _additionalInterestService.GetByRiskDetailIdActiveAsync(risk.CurrentRiskDetailId.Value)).ToList();
            reportData.Billing = billing;
            reportData.Endorsement = endorsement;

            if(!isFromEndorsement)
                await IncludeAINotices(endorsementForms, reportData);

            foreach (var endorsementForm in endorsementForms)
            {
                if (!isFromEndorsement)
                    reportData.AIWithNotice = endorsementForm.AIWithNotice;

                var packetFactory = new PolicyPacketPageFactory(riskDetailId, reportData, GetAttachedPages(endorsementForm.RiskForms));
                var formsIncluded = packetFactory.Factories.Values.Where(x => x.isAttached).ToList();

                var fileName = endorsementForm.FileName;
                var packetReportBuilder = ReportBuilder.Init();
                var reportPayload = packetReportBuilder.SetUploadPath($"{UPLOAD_PATH}/CA/{DateTime.Now:yyyy-MM}/{riskDetailId}/{fileName}.pdf")
                    .AddPage(formsIncluded)
                    .Build();

                var reportServiceSection = _configuration.GetSection("Report");
                var baseUrl = reportServiceSection.GetValue<string>("FileUrl");
                var policypacketdocument = new FileUploadDocumentDTO
                {
                    tableRefId = riskDetailId,
                    RiskDetailId = riskDetailId,
                    FileCategoryId = (short?)((endorsementForm.Description == "Final Audit Invoice" || endorsementForm.Description == "Final Invoice") ? 3 : 1),
                    Description = endorsementForm.Description,
                    FileName = $"{fileName}.pdf",
                    FilePath = $"{baseUrl}{reportPayload.Upload}",
                    IsUploaded = false,
                    IsSystemGenerated = true,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };
                var fileresult = await _fileUploadDocumentService.AddUpdateAsync(policypacketdocument);
                endorsementDocuments.Add(new { fileresult.Id, Payload = reportPayload });
            }

            if (sendToEmail)
            {
                var endorsementEmailContentData = await _endorsementEmailService.CreateContentData(riskDetailId);
                GenerateFile(endorsementDocuments, endorsementEmailContentData);
                return;
            }
            GenerateFile(endorsementDocuments, isAutoRescind);
        }

        public async Task GenerateNOPCDocuments(List<EndorsementFormDTO> endorsementForms, InvoiceDTO billing, EndorsementDetailsDTO endorsement, bool isAbove100k = false)
        {
            var riskDetailId = endorsement.Risk.Id.GetValueOrDefault();
            var endorsementDocuments = new List<dynamic>();
            var reportData = await _contentService.GetReportData(riskDetailId, true);
            var risk = await _riskRepository.GetRiskByIdAsync(endorsement.Risk.RiskId);

            reportData.Risk.RiskForms = await _riskFactory.GetRiskFormsWithConditionals(reportData.Risk, true);
            reportData.Risk.AdditionalInterest = (await _additionalInterestService.GetByRiskDetailIdActiveAsync(risk.CurrentRiskDetailId.Value)).ToList();
            reportData.Billing = billing;
            reportData.Endorsement = endorsement;

            await IncludeAINotices(endorsementForms, reportData);

            foreach (var endorsementForm in endorsementForms)
            {
                reportData.AIWithNotice = endorsementForm.AIWithNotice;

                var packetFactory = new PolicyPacketPageFactory(riskDetailId, reportData, GetAttachedPages(endorsementForm.RiskForms));
                var formsIncluded = packetFactory.Factories.Values.Where(x => x.isAttached).ToList();

                var fileName = endorsementForm.FileName;
                var packetReportBuilder = ReportBuilder.Init();
                var reportPayload = packetReportBuilder.SetUploadPath($"{UPLOAD_PATH}/CA/{DateTime.Now:yyyy-MM}/{riskDetailId}/{fileName}.pdf")
                    .AddPage(formsIncluded)
                    .Build();

                var reportServiceSection = _configuration.GetSection("Report");
                var baseUrl = reportServiceSection.GetValue<string>("FileUrl");
                var policypacketdocument = new FileUploadDocumentDTO
                {
                    tableRefId = riskDetailId,
                    RiskDetailId = riskDetailId,
                    FileCategoryId = (short?)((endorsementForm.Description == "Final Audit Invoice" || endorsementForm.Description == "Final Invoice") ? 3 : 1),
                    Description = endorsementForm.Description,
                    FileName = $"{fileName}.pdf",
                    FilePath = $"{baseUrl}{reportPayload.Upload}",
                    IsUploaded = false,
                    IsSystemGenerated = true,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };
                var fileresult = await _fileUploadDocumentService.AddUpdateAsync(policypacketdocument);
                endorsementDocuments.Add(new { fileresult.Id, Payload = reportPayload });
            }

            var NOPCEmailContentData = await _NOPCEmailService.CreateContentData(riskDetailId);
            GenerateFile(endorsementDocuments, NOPCEmailContentData, isAbove100k);
        }

        private void GenerateFile(IReadOnlyCollection<dynamic> documents, bool isAutoRescind = false)
        {
            var thread = new Thread(() =>
            {
                var contextFactory = new DbContextFactory();
                var connectionString = _configuration.GetSection("ConnectionStrings").GetValue<string>("ConCABoxTruck");
                using var context = contextFactory.Create(connectionString);

                foreach (var document in documents)
                {
                    try
                    {
                        var documentId = (Guid)document.Id;
                        var model = context.FileUploadDocument.FirstOrDefault(x => x.Id == documentId);
                        if (model == null) throw new Exception("File Upload Document not found");

                        // var logResult = _applogService.LogDocumentGeneration($"Document:{document.Id}", JsonConvert.SerializeObject(document.Payload), null).GetAwaiter().Result;
                        var message = $"Document:{document.Id}";
                        var jsonMessage = JsonConvert.SerializeObject(document.Payload);
                        context.AppLog.AddAsync(new AppLog()
                        {
                            UserId = Data.Common.Services.GetCurrentUser(),
                            Message = message,
                            RequestValues = jsonMessage,
                            Type = AppLogType.DocGen.ToString(),
                            CreatedDate = DateTime.Now,
                            ResponseValues = null
                        });

                        context.SaveChanges();

                        var result = _reportService.GenerateFileReportLink(document.Payload).Result;
                        if (result == null) throw new Exception($"Generate file unsuccessful:{model.RiskDetailId}");

                        model.IsUploaded = true;
                        context.FileUploadDocument.Update(model);

                        // Save to FTPDocument for Covenir
                        var source = "";
                        if ((model.Description?.Contains(GenerateDocumentDescription.Cancellation) ?? false) 
                        || (model.Description?.Contains(GenerateDocumentDescription.FinalCancellation) ?? false))
                            source = GenerateDocumentSource.Cancellation;

                        if ((model.Description?.Contains(GenerateDocumentDescription.Reinstatement) ?? false))
                            source = GenerateDocumentSource.Reinstatement;


                        if (!string.IsNullOrEmpty(source) && (model.Description != GenerateDocumentDescription.Reinstatement || !isAutoRescind))
                        {
                            var riskDetails = context.RiskDetail.FirstOrDefault(x => x.Id == model.RiskDetailId);
                            context.FTPDocumentTemporary.Add(new FTPDocumentTemporary()
                            {
                                RiskId = riskDetails.RiskId,
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
                            context.FTPDocument.Add(new FTPDocument()
                            {
                                RiskId = riskDetails.RiskId,
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
                        }


                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        _errorLogservice.Error(ex);
                        _applogService.LogDocumentGeneration($"Document:{document.Id}", JsonConvert.SerializeObject(document.Payload), "Generate file unsuccessful");

                    }
                }
            });

            thread.Start();
        }

        private void GenerateFile(IReadOnlyCollection<dynamic> documents, EndorsementEmailContentData emailContentData)
        {
            var thread = new Thread(() =>
            {
                var contextFactory = new DbContextFactory();
                var connectionString = _configuration.GetSection("ConnectionStrings").GetValue<string>("ConCABoxTruck");
                using var context = contextFactory.Create(connectionString);

                foreach (var document in documents)
                {
                    try
                    {
                        var documentId = (Guid)document.Id;
                        var model = context.FileUploadDocument.FirstOrDefault(x => x.Id == documentId);
                        if (model == null) throw new Exception("File Upload Document not found");

                        //var logResult = _applogService.LogDocumentGeneration($"Document:{document.Id}", JsonConvert.SerializeObject(document.Payload), null).GetAwaiter().Result;
                        var message = $"Document:{document.Id}";
                        var jsonMessage = JsonConvert.SerializeObject(document.Payload);
                        context.AppLog.AddAsync(new AppLog()
                        {
                            UserId = Data.Common.Services.GetCurrentUser(),
                            Message = message,
                            RequestValues = jsonMessage,
                            Type = AppLogType.DocGen.ToString(),
                            CreatedDate = DateTime.Now,
                            ResponseValues = null
                        });

                        context.SaveChanges();

                        var result = _reportService.GenerateFileReportLink(document.Payload).Result;
                        if (result == null) throw new Exception($"Generate file unsuccessful:{model.RiskDetailId}");

                        model.IsUploaded = true;
                        context.FileUploadDocument.Update(model);

                        context.SaveChanges();

                        // Create email queue with generated email content data for the generated invoice document.
                        if (!model.Description.Contains("Policy Packet"))
                        {
                            emailContentData.AddAttachment(result);
                            using var externalDbContext = _externalDbContextFactory.CreateDbContext();
                            EmailQueue emailQueue = _endorsementEmailService.CreateEmailQueueAsync(emailContentData).GetAwaiter().GetResult();
                            externalDbContext.EmailQueues.Add(emailQueue);
                            externalDbContext.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        _errorLogservice.Error(ex);
                        _applogService.LogDocumentGeneration($"Document:{document.Id}", JsonConvert.SerializeObject(document.Payload), "Generate file unsuccessful");

                    }
                }
            });

            thread.Start();
        }

        private void GenerateFile(IReadOnlyCollection<dynamic> documents, NOPCEmailContentData emailContentData, bool isAbove100k = false)
        {
            var thread = new Thread(() =>
            {
                var contextFactory = new DbContextFactory();
                var connectionString = _configuration.GetSection("ConnectionStrings").GetValue<string>("ConCABoxTruck");
                using var context = contextFactory.Create(connectionString);

                foreach (var document in documents)
                {
                    try
                    {
                        var documentId = (Guid)document.Id;
                        var model = context.FileUploadDocument.FirstOrDefault(x => x.Id == documentId);
                        if (model == null) throw new Exception("File Upload Document not found");

                        //var logResult = _applogService.LogDocumentGeneration($"Document:{document.Id}", JsonConvert.SerializeObject(document.Payload), null).GetAwaiter().Result().Result;
                        var message = $"Document:{document.Id}";
                        var jsonMessage = JsonConvert.SerializeObject(document.Payload);
                        context.AppLog.AddAsync(new AppLog()
                        {
                            UserId = Data.Common.Services.GetCurrentUser(),
                            Message = message,
                            RequestValues = jsonMessage,
                            Type = AppLogType.DocGen.ToString(),
                            CreatedDate = DateTime.Now,
                            ResponseValues = null
                        });

                        var result = _reportService.GenerateFileReportLink(document.Payload).Result;
                        if (result == null) throw new Exception($"Generate file unsuccessful:{model.RiskDetailId}");

                        model.IsUploaded = true;
                        context.FileUploadDocument.Update(model);

                        // Save to FTPDocument for Covenir
                        var source = "";
                        if ((model.Description?.Contains(GenerateDocumentDescription.NonPayNoticePendingCancellation) ?? false) && !isAbove100k)
                            source = GenerateDocumentSource.NonPayNoticePendingCancellation;
                         
                        if (!string.IsNullOrEmpty(source))
                        {
                            var riskDetails = context.RiskDetail.FirstOrDefault(x => x.Id == model.RiskDetailId);
                            context.FTPDocumentTemporary.Add(new FTPDocumentTemporary()
                            {
                                RiskId = riskDetails.RiskId,
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
                            context.FTPDocument.Add(new FTPDocument()
                            {
                                RiskId = riskDetails.RiskId,
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
                        }

                        context.SaveChanges();

                        //if (!isAbove100k)
                        //{
                        //    // Create email queue with generated email content data for the generated invoice document.
                        //    emailContentData.AddAttachment(result);
                        //    using var externalDbContext = _externalDbContextFactory.CreateDbContext();
                        //    EmailQueue emailQueue = _NOPCEmailService.CreateEmailQueueAsync(emailContentData).GetAwaiter().GetResult();
                        //    externalDbContext.EmailQueues.Add(emailQueue);
                        //    externalDbContext.SaveChanges();
                        //}
                    }
                    catch (Exception ex)
                    {
                        _errorLogservice.Error(ex);
                        _applogService.LogDocumentGeneration($"Document:{document.Id}", JsonConvert.SerializeObject(document.Payload), "Generate file unsuccessful");

                    }
                }
            });

            thread.Start();
        }

        private static IEnumerable<PageInfo> GetAttachedPages(IEnumerable<RiskFormDTO> forms)
        {
            var attachedForms = new List<PageInfo>();

            foreach (var form in forms)
            {
                attachedForms.Add(new PageInfo
                {
                    Id = form.Id.ToString(),
                    FormId = form.Form.Id,
                    PageOrder = form.Form.SortOrder,
                    TemplateName = form.Form.FileName
                });
            }

            return attachedForms;
        }

        public string RemoveInvalidChars(string filename)
        {
            return string.Concat(filename.Split(Path.GetInvalidFileNameChars()));
        }

        private async Task IncludeAINotices(List<EndorsementFormDTO> endorsementForms, ReportData reportData)
        {
            var formId = endorsementForms.FirstOrDefault().RiskForms.FirstOrDefault().Form.Id;
            var fileName = endorsementForms.FirstOrDefault().FileName;
            var description = endorsementForms.FirstOrDefault().Description;
            var aiWithNotices = reportData.Risk?.AdditionalInterest?
                .Where(a => a.Is30DayNotice)
                .GroupBy(p => new { p.Name, p.Address, p.State, p.City, p.ZipCode })
                .Select(g => g.First());

            if (aiWithNotices != null)
            {
                foreach(var aiWithNotice in aiWithNotices)
                {
                    //Set description convention for AI notice
                    var descriptionForAINotice = $"{description}, {aiWithNotice.Name}";

                    if (fileName.Contains("Cancellation_Notice_"))
                        fileName = $"Cancellation_Notice_{DateTime.Now:MMddyyyy_HHmmssfff}";

                    if (fileName.Contains("Notice_of_Pending_Cancellation_"))
                        fileName = $"Notice_of_Pending_Cancellation_{DateTime.Now:MMddyyyy_HHmmssfff}";

                    if (fileName.Contains("Reinstatement_Notice_"))
                        fileName = $"Reinstatement_Notice_{DateTime.Now:MMddyyyy_HHmmssfff}";

                    var aiNotice = new EndorsementFormDTO
                    {
                        Description = descriptionForAINotice,
                        FileName = fileName,
                        RiskForms = new List<RiskFormDTO> { new RiskFormDTO { Id = Guid.NewGuid(), Form = new FormDTO { Id = formId } } },
                        AIWithNotice = aiWithNotice
                    };

                    endorsementForms.Add(aiNotice);

                    await Task.Delay(100);
                }
            }
        }
    }
}
