using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.Constants;
using RivTech.CABoxTruck.WebService.Service.Services.ExcelManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class BindingService : IBindingService
    {
        private readonly IBindingRepository _BindingRepository;
        private readonly IBindingRequirementsRepository _BindingReqRepository;
        private readonly IFileUploadDocumentRepository _FileUploadRepository;
        private readonly IQuoteConditionsRepository _QuoteConditionsRepository;
        private readonly IFileUploadDocumentService _fileUploadDocumentService;
        private readonly ICacheService _cacheService;
        private readonly IStorageService _storageService;
        private readonly IApplogService _applogService;
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;

        private readonly IMapper _mapper;

        public BindingService(IBindingRepository bindingRepository,
            IBindingRequirementsRepository bindingReqRepository,
            IFileUploadDocumentRepository fileUploadDocumentRepository,
            IQuoteConditionsRepository quoteConditionsRepository,
            IStorageService storageService,
            IFileUploadDocumentService fileUploadDocumentService,
            ICacheService cacheService,
            IApplogService applogService,
            IRiskRepository riskRepository,
            IRiskDetailRepository riskDetailRepository,
            IMapper mapper)
        {
            _riskRepository = riskRepository;
            _riskDetailRepository = riskDetailRepository;
            _BindingRepository = bindingRepository;
            _BindingReqRepository = bindingReqRepository;
            _FileUploadRepository = fileUploadDocumentRepository;
            _QuoteConditionsRepository = quoteConditionsRepository;
            _storageService = storageService;
            _fileUploadDocumentService = fileUploadDocumentService;
            _cacheService = cacheService;
            _applogService = applogService;
            _mapper = mapper;
        }

        #region BINDINGS
        public async Task<BindingDTO> AddUpdateAsync(BindingDTO model)
        {
            var obj = new Binding();
            try
            {
                var riskDetail = await _riskRepository.GetAsync(x => x.Id == model.RiskId);
                var status = riskDetail.FirstOrDefault().Status;
                var bindingData = await _BindingRepository.FindAsync(x => x.RiskId == model.RiskId);

                if (bindingData != null)
                {
                    bindingData.UpdatedDate = DateTime.Now;
                    bindingData.BindOptionId = model.BindOptionId;
                    bindingData.SLANum = model.SLANum;
                    bindingData.PaymentTypesId = model.PaymentTypesId;
                    bindingData.ProducerSLNumber = model.ProducerSLNumber;
                    bindingData.SLState = model.SLState;

                    if (status != "Canceled" && status != "Active" && status != "Expired")
                    {
                        bindingData.MinimumEarned = model.MinimumEarned;
                        bindingData.SurplusLIneNum = model.SurplusLIneNum;
                        bindingData.CreditedOfficeId = model.CreditedOfficeId;
                    }

                    obj = _BindingRepository.Update(bindingData);
                    await _BindingRepository.SaveChangesAsync();
                }
                else
                {
                    model.Id = Guid.NewGuid();
                    model.CreatedDate = DateTime.Now;

                    obj = await _BindingRepository.AddAsync(_mapper.Map<Binding>(model));
                    await _BindingRepository.SaveChangesAsync();
                }

            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
                await _BindingRepository.SaveChangesAsync();
            }
            catch (Exception ex) { throw; }

            return _mapper.Map<BindingDTO>(obj);
        }


        public async Task<BindingDTO> GetBindingByRiskIdAsync(Guid id)
        {
            var result = await _BindingRepository.FindAsync(x => x.RiskId == id);
            return _mapper.Map<BindingDTO>(result);
        }

        #endregion BINDINGS


        #region BINDING REQUIREMENTS


        public async Task<List<BindingRequirementsDTO>> GetAllAsync()
        {
            var result = await _BindingReqRepository.GetAllAsync();
            return _mapper.Map<List<BindingRequirementsDTO>>(result);
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _BindingReqRepository.AnyAsync(x => x.Id == id);
        }

        public async Task<BindingRequirementsDTO> GetByIdAsync(Guid id)
        {
            var result = await _BindingReqRepository.FindAsync(x => x.Id == id);
            return _mapper.Map<BindingRequirementsDTO>(result);
        }

        public async Task<List<BindingRequirementsDTO>> GetByRiskDetailIdAsync(Guid id)
        {
            var result = await _BindingReqRepository.GetAsync(x => x.RiskDetailId == id && x.IsActive != false);
            var res = result.Select(x => _mapper.Map<BindingRequirementsDTO>(x)).ToList();

            foreach (var bind in res)
            {
                var documentLink = _fileUploadDocumentService.GetByRefIdAsync(bind.Id.Value).Result.FirstOrDefault();
                if (documentLink != null)
                    bind.FilePath = documentLink.FilePath;
            }

            return res.ToList();
        }

        public async Task<BindingRequirementsDTO> AddUpdateAsync(BindingRequirementsDTO model)
        {
            try
            {
                var obj = _mapper.Map<BindingRequirements>(model);

                if (model?.Id == null)
                {
                    var exist = (await _BindingReqRepository
                        .GetAsync(o =>
                            (o.BindRequirementsOptionId != null && o.BindRequirementsOptionId == obj.BindRequirementsOptionId && o.BindRequirementsOptionId != 16) && 
                            o.RiskDetailId == obj.RiskDetailId &&
                            o.IsActive != false
                        )).Any();

                    if(exist)
                    {
                        throw new Exception("Duplicate entries not allowed.");
                    }

                    var result = await _BindingReqRepository.AddAsync(obj);
                    await _BindingReqRepository.SaveChangesAsync();

                    model = _mapper.Map<BindingRequirementsDTO>(result);
                }
                else
                {
                    var result = (await _BindingReqRepository.GetAsync(o => o.Id == obj.Id)).FirstOrDefault();

                    result.BindRequirementsOptionId = obj.BindRequirementsOptionId;
                    result.IsPreBind = obj.IsPreBind;

                    result.BindStatusId = obj.BindStatusId;
                    result.Describe = obj.Describe;
                    result.Comments = obj.Comments;
                    result.RelevantDocumentDesc = obj.RelevantDocumentDesc;

                    result.UpdatedDate = DateTime.Now.ToLocalTime();

                    _BindingReqRepository.Update(result);

                    //var objAttachment = _mapper.Map<List<FileUploadDocumentDTO>>(model.FileUploads);

                    if (model.FileUploads != null && model.FileUploads.Any(x => x.IsUploaded == true))
                    {
                        #region tag as disabled old files
                        var old = _FileUploadRepository.GetAsync(i => i.tableRefId == result.Id).Result;

                        foreach (var f in old)
                        {
                            f.IsActive = false;
                            f.DeletedDate = DateTime.Now;
                            _FileUploadRepository.Entry<FileUploadDocument>(f).Property(p => p.IsActive).IsModified = true;
                            _FileUploadRepository.Entry<FileUploadDocument>(f).Property(p => p.DeletedDate).IsModified = true;

                            //after tagging delete. what will happen to previously uploaded batch of files. 
                        }

                        #endregion

                        //var files = objAttachment.Select(s => _mapper.Map<FileUploadDocument>(s)).ToList();
                        var files = _mapper.Map<List<FileUploadDocument>>(model.FileUploads);
                        files.ForEach(f => { 
                            f.tableRefId = result.Id;
                            f.RiskDetailId = result.RiskDetailId;
                            f.FileCategoryId = FileCategory.BindRequirement.Id;
                            f.CreatedDate = DateTime.Now;
                            f.CreatedBy = Data.Common.Services.GetCurrentUser();
                        });
                        await _FileUploadRepository.AddRangeAsync(files);
                    }
                    else
                    {
                        var files = (await _FileUploadRepository.GetAsync(o => o.tableRefId == result.Id && o.IsActive != false));

                        files.ToList().ForEach(f =>
                        {
                            if (result.RelevantDocumentDesc != f.Description)
                            {
                                f.Description = result.RelevantDocumentDesc;
                                _FileUploadRepository.Entry<FileUploadDocument>(f).Property(p => p.Description).IsModified = true;
                            }
                        });
                    }

                    await _BindingReqRepository.SaveChangesAsync(); // triggers all the savings and edits
                    model = _mapper.Map<BindingRequirementsDTO>(result);
                }

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> RemoveAsync(Guid id)
        {
            var entity = await _BindingReqRepository.FindAsync(x => x.Id == id);
            entity.IsActive = false;
            entity.DeletedDate = DateTime.Now;
            await _BindingReqRepository.SaveChangesAsync();

            var file = await _FileUploadRepository.FindAsync(x => x.tableRefId == id);
            if (file != null)
            {
                file.IsActive = false;
                file.DeletedDate = DateTime.Now;
                _FileUploadRepository.Update(file);
                await _FileUploadRepository.SaveChangesAsync();
            }

            return id.ToString();
        }

        public async Task<List<BindingRequirementsDTO>> GetByRiskDetailIdIncludeAsync(Guid id)
        {
            var result = await _BindingReqRepository.GetAllIncludeAsync(x => x.RiskDetailId == id && x.IsActive != false);
            return _mapper.Map<List<BindingRequirementsDTO>>(result).ToList();
        }

        public async Task<List<BindingRequirementsDTO>> GetAllIncludeAsync()
        {
            var result = await _BindingReqRepository.GetAllIncludeAsync();
            return _mapper.Map<List<BindingRequirementsDTO>>(result).ToList();

        }

        public async Task<List<BindingRequirementsDTO>> GetByIdIncludeAsync(Guid id)
        {
            var result = await _BindingReqRepository.GetAllIncludeAsync(o => o.Id == id);
            return _mapper.Map<List<BindingRequirementsDTO>>(result).ToList();
        }

        public async Task<List<BindingRequirementsDTO>> InsertRangeAsync(List<BindingRequirementsDTO> model)
        {
            var objs = _mapper.Map<List<BindingRequirements>>(model);
            foreach (var obj in objs)
            {
                obj.Id = Guid.NewGuid();
                obj.CreatedDate = DateTime.Now;
            }

            await _BindingReqRepository.AddRangeAsync(objs);
            await _BindingReqRepository.SaveChangesAsync();

            return _mapper.Map<List<BindingRequirementsDTO>>(objs);
        }

        #endregion BINDING REQUIREMENTS

        #region Quote Conditions
        public async Task<QuoteConditionsDTO> AddUpdateAsync(QuoteConditionsDTO model)
        {
            try
            {
                var obj = _mapper.Map<QuoteConditions>(model);

                if (model?.Id == null)
                {
                    var exist = (await _QuoteConditionsRepository
                        .GetAsync(o => 
                            //base on requirements bind requirement is optional? 10 == others
                            (o.QuoteConditionsOptionId != null && o.QuoteConditionsOptionId == obj.QuoteConditionsOptionId && o.QuoteConditionsOptionId != 10) &&
                            o.RiskDetailId == obj.RiskDetailId &&
                            o.IsActive != false
                        )).Any();

                    if (exist)
                    {
                        throw new Exception("Duplicate entries not allowed.");
                    }

                    var result = await _QuoteConditionsRepository.AddAsync(obj);
                    await _QuoteConditionsRepository.SaveChangesAsync();

                    model = _mapper.Map<QuoteConditionsDTO>(result);
                }
                else
                {
                    var result = (await _QuoteConditionsRepository.GetAsync(o => o.Id == obj.Id)).FirstOrDefault();

                    result.QuoteConditionsOptionId = obj.QuoteConditionsOptionId;
                    result.Describe = obj.Describe;
                    result.Comments = obj.Comments;
                    result.RelevantDocumentDesc = obj.RelevantDocumentDesc;

                    result.UpdatedDate = DateTime.Now.ToLocalTime();

                    _QuoteConditionsRepository.Update(result);

                    if (model.FileUploads != null && model.FileUploads.Any(x => x.IsUploaded == true))
                    {
                        #region tag as disabled old files
                        var old = _FileUploadRepository.GetAsync(i => i.tableRefId == result.Id).Result;

                        foreach (var f in old)
                        {
                            f.IsActive = false;
                            f.DeletedDate = DateTime.Now;
                            _FileUploadRepository.Entry<FileUploadDocument>(f).Property(p => p.IsActive).IsModified = true;
                            _FileUploadRepository.Entry<FileUploadDocument>(f).Property(p => p.DeletedDate).IsModified = true;

                            //after tagging delete. what will happen to previously uploaded batch of files. 
                        }

                        #endregion

                        var files = _mapper.Map<List<FileUploadDocument>>(model.FileUploads);
                        files.ForEach(f => { 
                            f.tableRefId = result.Id;
                            f.RiskDetailId = result.RiskDetailId;
                            f.FileCategoryId = FileCategory.QuoteCondition.Id;
                            f.CreatedDate = DateTime.Now;
                            f.CreatedBy = Data.Common.Services.GetCurrentUser();
                        });
                        await _FileUploadRepository.AddRangeAsync(files);
                    }
                    else
                    {
                        var files = (await _FileUploadRepository.GetAsync(o => o.tableRefId == result.Id && o.IsActive != false));

                        files.ToList().ForEach(f =>
                        {
                            if (result.RelevantDocumentDesc != f.Description)
                            {
                                f.Description = result.RelevantDocumentDesc;
                                _FileUploadRepository.Entry<FileUploadDocument>(f).Property(p => p.Description).IsModified = true;
                            }
                        });
                    }

                    await _QuoteConditionsRepository.SaveChangesAsync(); // triggers all the savings and edits
                    model = _mapper.Map<QuoteConditionsDTO>(result);
                }
            }
            catch (Exception ex)
            {
                //await _logError(ex.Message, ex.InnerException.ToString());
            }

            var quoteCondition = await GetQuoteByIdAsync((Guid)model.Id);
            return quoteCondition;
        }

        public async Task<List<QuoteConditionsDTO>> GetQuoteByRiskDetailIdAsync(Guid id)
        {
            var result = await _QuoteConditionsRepository.GetAsync(x => x.RiskDetailId == id && x.IsActive != false, include => include.QuoteConditionsOption);
            var res = result.Select(x => _mapper.Map<QuoteConditionsDTO>(x)).ToList();

            foreach (var quote in res)
            {
                var documentLink = _fileUploadDocumentService.GetByRefIdAsync(quote.Id.Value).Result.FirstOrDefault();
                if (documentLink != null)
                    quote.FilePath = documentLink.FilePath;
            }

            return res.ToList();
        }

        public async Task<int[]> GetQuoteConditionOptionIsExist(Guid id, int[] quoteConditionOptionIds)
        {
            var quoteConditions = await _QuoteConditionsRepository.GetAsync(x => x.RiskDetailId == id && quoteConditionOptionIds.Contains(x.QuoteConditionsOptionId ?? -1));
            var result = Array.ConvertAll(quoteConditions.Distinct().Select(q => q.QuoteConditionsOptionId).ToArray(), element => (int)element);

            return result;
        }

        public async Task<string> RemoveQuoteAsync(Guid id)
        {
            try
            {
                var entity = await _QuoteConditionsRepository.FindAsync(x => x.Id == id);
                entity.IsActive = false;
                entity.DeletedDate = DateTime.Now;
                await _BindingReqRepository.SaveChangesAsync();

                var file = await _FileUploadRepository.FindAsync(x => x.tableRefId == id);
                if (file != null)
                {
                    file.IsActive = false;
                    file.DeletedDate = DateTime.Now;
                    _FileUploadRepository.Update(file);
                    await _FileUploadRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                //await _logError(ex.Message, ex.InnerException.ToString());
            }

            return id.ToString();
        }
        public async Task<QuoteConditionsDTO> GetQuoteByIdAsync(Guid id)
        {
            return _mapper.Map<QuoteConditionsDTO>(await _QuoteConditionsRepository.FindAsync(x => x.Id == id, include => include.QuoteConditionsOption));
        }

        public async Task<List<QuoteConditionsDTO>> SetQuoteConditionOptions(QuoteConditionRequestDTO quoteConditionRequest)
        {
            var quoteConditions = await _cacheService.GetKeyValueList("QuoteConditionsOption");
            var riskDetailQuoteConditionsWithDeleted = await _QuoteConditionsRepository.GetAsync(x => x.RiskDetailId == quoteConditionRequest.Id);

            var hasSymbolTen = quoteConditionRequest.RiskCoverage?.ALSymbolId == Data.Entity.DataSets.Symbol.Symbol10_1.Id
                            || quoteConditionRequest.RiskCoverage?.ALSymbolId == Data.Entity.DataSets.Symbol.Symbol10_2.Id;

            var hasAPD = (quoteConditionRequest.RiskCoverage?.CompDeductibleId ?? LimitConstants.ComprehensiveNoCoverageId) != LimitConstants.ComprehensiveNoCoverageId
                || (quoteConditionRequest.RiskCoverage?.CollDeductibleId ?? LimitConstants.CollisionNoCoverageId) != LimitConstants.CollisionNoCoverageId
                || (quoteConditionRequest.RiskCoverage?.FireDeductibleId ?? LimitConstants.ComprehensiveNoCoverageId) != LimitConstants.ComprehensiveNoCoverageId;

            
            bool isS37KORulesUpdateApplicable = await IsFeatureApplicable(quoteConditionRequest.Id, "S37KORulesUpdate");

            foreach (QuoteConditionsOption quoteCondition in quoteConditions)
            {
                bool excludeFromCheck = false;

                if (!quoteCondition.IsActive)
                    continue;

                bool isExist = riskDetailQuoteConditionsWithDeleted.Any(cq => cq.QuoteConditionsOptionId == quoteCondition.Id);

                //Exclude other
                if (quoteCondition.Id == QuoteConditionOptionsConstants.Other)
                    continue;

                //Exclude Dirver violation
                if (quoteCondition.Id == QuoteConditionOptionsConstants.DriverOnlyHasOneViolation && isS37KORulesUpdateApplicable)
                    continue;

                //Only check Physdam and SymbolTen in policy
                if (quoteConditionRequest.IsPolicy && (quoteCondition.Id != QuoteConditionOptionsConstants.PhysDamLimitOfInsurance && quoteCondition.Id != QuoteConditionOptionsConstants.SymbolTen))
                    continue;

                if (quoteCondition.Id == QuoteConditionOptionsConstants.PhysDamLimitOfInsurance)
                {
                    var activePhysDam = riskDetailQuoteConditionsWithDeleted.Where(rdqc => rdqc.IsActive != false &&
                    rdqc.QuoteConditionsOptionId == QuoteConditionOptionsConstants.PhysDamLimitOfInsurance);

                    if (!hasAPD)
                    {
                        await UnlinkQuoteConditionDocuments(activePhysDam);
                        continue;
                    }
                    else if(!activePhysDam.Any())
                        excludeFromCheck = true;
                }

                if (quoteCondition.Id == QuoteConditionOptionsConstants.SymbolTen)
                {
                    var activeSymbolTen = riskDetailQuoteConditionsWithDeleted.Where(rdqc => rdqc.IsActive != false && 
                    rdqc.QuoteConditionsOptionId == QuoteConditionOptionsConstants.SymbolTen);

                    if (!hasSymbolTen)
                    {
                        await UnlinkQuoteConditionDocuments(activeSymbolTen);
                        continue;
                    }
                    else if(!activeSymbolTen.Any())
                        excludeFromCheck = true;
                }

                if (riskDetailQuoteConditionsWithDeleted.Any() && isExist && !excludeFromCheck)
                    continue;

                var quoteConditionDTO = new QuoteConditionsDTO
                {
                    QuoteConditionsOptionId = quoteCondition.Id,
                    IsDefault = true,
                    RiskDetailId = quoteConditionRequest.Id
                };

                await AddUpdateAsync(quoteConditionDTO);
            }

            var result = await GetQuoteByRiskDetailIdAsync(quoteConditionRequest.Id);

            return result;
        }

        private async Task UnlinkQuoteConditionDocuments(IEnumerable<QuoteConditions> activeQuoteConditions)
        {
            if (activeQuoteConditions.Any())
            {
                foreach (var activeQuoteCondition in activeQuoteConditions)
                {
                    var activequoteConditionDTO = _mapper.Map<QuoteConditionsDTO>(activeQuoteCondition);
                    var documentLink = await _fileUploadDocumentService.GetByRefIdAsync(activeQuoteCondition.Id);
                    if (documentLink.Any())
                    {
                        activequoteConditionDTO.FileUploads = new List<FileUploadDocumentDTO>();

                        activequoteConditionDTO.FileUploads.AddRange(documentLink);
                    }

                    await RemoveQuoteAsync(activeQuoteCondition.Id);
                    var temp = JsonConvert.SerializeObject(activeQuoteCondition);
                    await _applogService.LogDocumentGeneration($"QuoteConditionDelete:{activequoteConditionDTO.RiskDetailId}", "RemoveAsync", temp);
                }
            }
        }

        private async Task<bool> IsFeatureApplicable(Guid riskDetailId, string fieldName)
        {
            var fieldVersions = await _riskRepository.GetFieldVersions();
            var riskId = await _riskDetailRepository.GetRiskIdAsync(riskDetailId);
            var risk = await _riskRepository.FindAsync(riskId);
            var submissionRiskDetail = await _riskDetailRepository.GetFirstRiskDetailByRiskIdAsync(riskId);
            var isApplicable = fieldVersions.Any(v => v.FieldName == fieldName && submissionRiskDetail.RequestedQuoteDate >= v.EffectiveDate);

            if (submissionRiskDetail.RequestedQuoteDate == null && !risk.IsRaterVersionLocked)
                return fieldVersions.Any(v => v.FieldName == fieldName);

            return isApplicable;
        }
        #endregion

        #region GENERIC functions

        public async Task<T> AzureBlogSave<T>(IFormFileCollection formFiles, string States, string type, T resource) where T : AzureBlobSaveDTO
        {
            if (formFiles.Any())
            {
                var tasks = new List<Task>();
                int failed = 0;

                foreach (IFormFile postedFile in formFiles)
                {
                    tasks.Add(Task.Run(async () => {
                        var fileDto = resource.FileUploads.ToList().Find(f => f.FileName == postedFile.FileName);

                        fileDto.CreatedDate = DateTime.Now;

                        var Id = Guid.NewGuid();
                        //var newFileName = $"{Id}{Path.GetExtension(postedFile.FileName)}";
                        var folder = !string.IsNullOrEmpty(type) ? type : "others";
                        var dte = $"{resource.CreatedDate?.Year}-{resource.CreatedDate?.ToString("MM")}";
                        var filePath = $"{dte}/{resource.RiskDetailId}/{folder}/{postedFile.FileName}";

                        fileDto.MimeType = postedFile.ContentType;
                        fileDto.FileExtension = Path.GetExtension(postedFile.FileName).Replace(".", "");
                        //fileDto.FilePath = $"{States?.ToUpper()}/{filePath}";

                        try
                        {
                            var pathResult = await _storageService.Upload(postedFile, filePath, _FOLDER_PATH: States?.ToUpper());
                            fileDto.FilePath = pathResult;
                            fileDto.IsUploaded = true;
                            fileDto.CreatedBy = Data.Common.Services.GetCurrentUser();
                        }
                        catch (Exception ex)
                        {
                            fileDto.IsUploaded = true;
                        }
                    }));
                }

                Task t = Task.WhenAll(tasks.ToArray());

                try
                {
                    await t.ConfigureAwait(true);
                }

                catch { }

                if (t.Status == TaskStatus.RanToCompletion)
                {
                    //all task for saving to azure saved.
                }
                else if (t.Status == TaskStatus.Faulted)
                {
                    //unsaved
                }
            }

            return resource;
        }

        public async Task<T> AzureBlogSave<T>(byte[] zipFile, string States, string type, T resource) where T : AzureBlobSaveDTO
        {
            var tasks = new List<Task>();
            int failed = 0;

            tasks.Add(Task.Run(async () => {
                var files = new List<FileUploadDocumentDTO>();
                var fileDto = resource.FileUploads.ToList().FirstOrDefault();
                fileDto.FileName = string.Join("\r\n", resource.FileUploads.Select(x => x.FileName).ToList());
                fileDto.CreatedDate = DateTime.Now;
                var filename = $"{DateTime.Now:MMddyyyyhhmmssff}.zip";
                var Id = Guid.NewGuid();
                var folder = !string.IsNullOrEmpty(type) ? type : "others";
                var dte = $"{resource.CreatedDate?.Year}-{resource.CreatedDate?.ToString("MM")}";
                var filePath = $"{dte}/{resource.RiskDetailId}/{folder}/{filename}";

                try
                {
                    var pathResult = await _storageService.Upload(zipFile, filePath, _FOLDER_PATH: States?.ToUpper());
                    fileDto.FilePath = pathResult;
                    fileDto.IsUploaded = true;
                    fileDto.CreatedBy = Data.Common.Services.GetCurrentUser();
                }
                catch (Exception ex)
                {
                    fileDto.IsUploaded = true;
                }
                files.Add(fileDto);
                resource.FileUploads = files;
            }));

            Task t = Task.WhenAll(tasks.ToArray());

            try
            {
                await t.ConfigureAwait(true);
            }

            catch { }

            if (t.Status == TaskStatus.RanToCompletion)
            {
                //all task for saving to azure saved.
            }
            else if (t.Status == TaskStatus.Faulted)
            {
                //unsaved
            }

            return resource;
        }

        #endregion GENERIC functions
    }
}
