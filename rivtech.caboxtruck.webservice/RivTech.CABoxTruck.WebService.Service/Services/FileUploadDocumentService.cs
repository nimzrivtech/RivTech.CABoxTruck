using AutoMapper;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Wordprocessing;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Billing;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class FileUploadDocumentService : IFileUploadDocumentService
    {
        private readonly IFileUploadDocumentRepository _FileUploadRepository;
        private readonly IBindingRequirementsRepository _bindingRequirementsRepository;
        private readonly IQuoteConditionsRepository _quoteConditionsRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IApplogService _applogService;
        private readonly ISASURLGeneratorService _SASURLGeneratorService;
        private readonly ILog4NetService _logger;
        private readonly IStorageService _storageService;

        private readonly IMapper _mapper;

        public FileUploadDocumentService(IFileUploadDocumentRepository fileUploadRepository,
            IBindingRequirementsRepository bindingRequirementsRepository,
            IQuoteConditionsRepository quoteConditionsRepository,
            IRiskDetailRepository riskDetailRepository,
            IMapper mapper,
            IApplogService applogService,
            ISASURLGeneratorService SASURLGeneratorService,
            IStorageService storageService,
            ILog4NetService logger)
        {
            _FileUploadRepository = fileUploadRepository;
            _bindingRequirementsRepository = bindingRequirementsRepository;
            _quoteConditionsRepository = quoteConditionsRepository;
            _riskDetailRepository = riskDetailRepository;
            _mapper = mapper;
            _applogService = applogService;
            _SASURLGeneratorService = SASURLGeneratorService;
            _logger = logger;
            _storageService = storageService;
        }

        public async Task<List<FileUploadDocumentDTO>> GetAllAsync()
        {
            var result = await _FileUploadRepository.GetAllAsync();
            return _mapper.Map<List<FileUploadDocumentDTO>>(result);
        }

        public async Task<List<FileUploadDocumentDTO>> GetByRefIdAsync(Guid id)
        {
            var result = await _FileUploadRepository.GetAsync(x => x.tableRefId == id && x.IsActive != false);
            return _mapper.Map<List<FileUploadDocumentDTO>>(result);
        }

        public async Task<List<FileUploadDocumentDTO>> GetByRiskDetailIdAsync(Guid id)
        {
            var result = new List<FileUploadDocumentDTO>();
            var fileUploadDocuments = await _FileUploadRepository.GetAsync(x => x.RiskDetailId == id && x.IsActive != false);
            foreach(var item in fileUploadDocuments.ToList())
            {
                var bind = await _bindingRequirementsRepository.FindAsync(x => x.Id == item.tableRefId);
                if (bind != null && bind.BindStatusId == BindStatus.Pending.Id)
                    continue;

                if (!CheckIsAllowedEditDeleteAsync(item.Id).Result)
                    item.IsSystemGenerated = true;

                result.Add(_mapper.Map<FileUploadDocumentDTO>(item));
            }

            return result;
        }

        public async Task<List<FileUploadDocumentDTO>> GetByRiskIdAsync(Guid id)
        {
            var result = new List<FileUploadDocumentDTO>();
            var riskDetailIds = _riskDetailRepository.GetAsync(x => x.RiskId == id).Result.Select(x => x.Id).ToList();
            var fileUploadDocuments = await _FileUploadRepository.GetAsync(x => riskDetailIds.Contains(x.RiskDetailId.Value) && x.IsActive != false);
            foreach (var item in fileUploadDocuments.ToList())
            {
                var bind = await _bindingRequirementsRepository.FindAsync(x => x.Id == item.tableRefId);
                if (bind != null && bind.BindStatusId == BindStatus.Pending.Id)
                    continue;


                if (!CheckIsAllowedEditDeleteAsync(item.Id).Result)
                    item.IsSystemGenerated = true;

                result.Add(_mapper.Map<FileUploadDocumentDTO>(item));
            }

            return result;
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _FileUploadRepository.AnyAsync(x => x.Id == id);
        }

        public async Task<FileUploadDocumentDTO> GetByIdAsync(Guid id)
        {
            var result = await _FileUploadRepository.FindAsync(x => x.Id == id);
            return _mapper.Map<FileUploadDocumentDTO>(result);
        }

        public async Task<List<FileUploadDocumentDTO>> GetByIdsAsync(List<Guid> ids)
        {
            var result = await _FileUploadRepository.GetAsync(x => ids.Contains(x.Id));
            return _mapper.Map<List<FileUploadDocumentDTO>>(result);
        }

        public async Task<FileUploadDocumentDTO> AddUpdateAsync(FileUploadDocumentDTO model)
        {

            try
            {
                var obj = _mapper.Map<FileUploadDocument>(model);

                if (model?.Id == null)
                {
                    var result = await _FileUploadRepository.AddAsync(obj);
                    await _FileUploadRepository.SaveChangesAsync();

                    model = _mapper.Map<FileUploadDocumentDTO>(result);
                }
                else
                {
                    var result = (await _FileUploadRepository.GetAsync(o => o.Id == obj.Id)).FirstOrDefault();

                    result.FileCategoryId = obj.FileCategoryId;
                    result.Description = obj.Description;

                    result.FileName = obj.FileName;
                    result.FilePath = obj.FilePath;
                    result.IsConfidential = obj.IsConfidential;

                    result.IsUploaded = obj.IsUploaded;
                    result.MimeType = obj.MimeType;
                    result.FileExtension = obj.FileExtension;
                    //result.CreatedBy = obj.CreatedBy;

                    result.UpdatedDate = DateTime.Now;

                    _FileUploadRepository.Update(result);
                    await _FileUploadRepository.SaveChangesAsync();

                    model = _mapper.Map<FileUploadDocumentDTO>(result);
                }

                var temp = JsonConvert.SerializeObject(model);

                await _applogService.LogDocumentGeneration($"DocumentUpload:{model.RiskDetailId}", "AddUpdateAsync", temp);

                return model;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> RemoveAsync(Guid id)
        {
            var entity = await _FileUploadRepository.FindAsync(x => x.Id == id);
            entity.IsActive = false;
            entity.DeletedDate = DateTime.Now;
            await _FileUploadRepository.SaveChangesAsync();

            if (entity.FileCategoryId == FileCategory.BindRequirement.Id)
            {
                var bind = await _bindingRequirementsRepository.FindAsync(x => x.Id == entity.tableRefId);
                if (bind != null)
                {
                    bind.RelevantDocumentDesc = string.Empty;
                    _bindingRequirementsRepository.Update(bind);
                    await _bindingRequirementsRepository.SaveChangesAsync();
                }
            }

            if (entity.FileCategoryId == FileCategory.QuoteCondition.Id)
            {
                var quote = await _quoteConditionsRepository.FindAsync(x => x.Id == entity.tableRefId);
                if (quote != null)
                {
                    quote.RelevantDocumentDesc = string.Empty;
                    _quoteConditionsRepository.Update(quote);
                    await _quoteConditionsRepository.SaveChangesAsync();
                }
            }

            var temp = JsonConvert.SerializeObject(entity);

            await _applogService.LogDocumentGeneration($"DocumentDelete:{entity.RiskDetailId}", "RemoveAsync", temp); ;

            return id.ToString();
        }

        public async Task<FileUploadDocumentDTO> uploadAzure(Stream fileStream)
        {
            var obj = new FileUploadDocumentDTO();
            //process azure
            return obj;
        }

        public async Task<bool> CheckIsUploadedByIdAsync(Guid id)
        {
            var result = await _FileUploadRepository.FindAsync(x => x.Id == id);
            return result.IsUploaded.Value;
        }

        public async Task<bool> CheckIsAllowedEditDeleteAsync(Guid id)
        {
            var result = await _FileUploadRepository.FindAsync(x => x.Id == id);
            if (result != null)
            {
                var expDate = result.CreatedDate.AddDays(1).Date;
                if (DateTime.Now.Date >= expDate)
                    return false;

                if (result.CreatedBy != Data.Common.Services.GetCurrentUser())
                    return false;
            }

            return true;
        }

        public async Task<bool> IsDocumentDuplicateFileNameAsync(Guid riskDetailId, List<string> fileNames, Guid? id)
        {
            var risk = await _riskDetailRepository.FindAsync(x => x.Id == riskDetailId);
            var riskDetailIds = _riskDetailRepository.GetAsync(x => x.RiskId == risk.RiskId).Result.Select(x => x.Id).ToList();
            foreach(var rdetailId in riskDetailIds)
            {
                var result = (await _FileUploadRepository.GetAsync(x => x.RiskDetailId == rdetailId && x.DeletedDate == null)).ToList();
                if (result.Count > 0)
                {
                    if (id.HasValue)
                        result = result.Where(x => x.Id != id.Value).ToList();

                    foreach (var fileName in fileNames)
                    {
                        if (result.Any(x => x.FileName.ToLower() == fileName.ToLower()))
                            return true;

                        if (result.Any(x => x.FileName.ToLower().IndexOf(fileName.ToLower()) >= 0))
                            return true;
                    }
                }
            }

            return false;
        }

        //public async Task<bool> IsBindReqDuplicateFileNameAsync(Guid riskDetailId, List<string> fileNames, Guid? id)
        //{
        //    var ids = new List<Guid>();
        //    var bindReqOthers = (await _bindingRequirementsRepository.GetAsync(x => x.RiskDetailId == riskDetailId)).Select(x => x.Id).ToList();
        //    if (bindReqOthers.Count > 0)
        //    {
        //        if (id.HasValue)
        //            ids = bindReqOthers.Where(x => x != id.Value).ToList();

        //        var result = (await _FileUploadRepository.GetAsync(x => ids.Contains(x.tableRefId.Value) && x.DeletedDate == null)).ToList();
        //        if (result.Count > 0)
        //        {
        //            foreach(var fileName in fileNames)
        //            {
        //                if (result.Any(x => x.FileName.ToLower() == fileName.ToLower()))
        //                    return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        public async Task<bool> IsBindQuoteDuplicateFileNameAsync(Guid riskDetailId, List<string> fileNames, Guid? id)
        {
            var result = (await _FileUploadRepository.GetAsync(x => x.RiskDetailId == riskDetailId && x.DeletedDate == null)).ToList();
            if (result.Count > 0)
            {
                if (id.HasValue)
                    result = result.Where(x => x.tableRefId != id.Value).ToList();
                foreach (var fileName in fileNames)
                {
                    if (result.Any(x => x.FileName.ToLower() == fileName.ToLower()))
                        return true;

                    if (result.Any(x => x.FileName.ToLower().IndexOf(fileName.ToLower()) >= 0))
                        return true;
                }
            }

            return false;
        }

        public byte[] CreateZipFile(IFormFileCollection formFiles)
        {
            byte[] compressedBytes;

            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in formFiles)
                    {
                        var fileInArchive = archive.CreateEntry(file.FileName, CompressionLevel.Optimal);
                        using (var entryStream = fileInArchive.Open())
                        file.CopyTo(entryStream);
                    }
                }
                compressedBytes = outStream.ToArray();
            }

            return compressedBytes;
        }

        public async Task<FileUploadPagingDTO> GetByRiskIdAsync(FileUploadDocFilterDTO fileuploadfilter)
        {
            DateTime d;

            bool chValidity = DateTime.TryParseExact(
            fileuploadfilter.Keyword,
            "MM/dd/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out d);

            DateTime? startdtKeyword = chValidity ? Convert.ToDateTime(fileuploadfilter.Keyword +" 00:00") : null;
            DateTime? enddtKeyword = chValidity ? Convert.ToDateTime(fileuploadfilter.Keyword + " 12:59" ) : null;

            Expression<Func<FileUploadDocument, bool>> expression;
            if (fileuploadfilter.Keyword == null || fileuploadfilter.Keyword.Length<=0)
            {
                expression = !fileuploadfilter.isPolicy ?
                                x => x.RiskDetailId == fileuploadfilter.RiskDetailId && x.IsActive != false && x.FileCategoryId != null :
                                fileuploadfilter.isBilling ?
                                    x => x.RiskDetail.RiskId == fileuploadfilter.RiskDetailId && x.IsActive != false && x.FileCategoryId != null && x.FileCategoryId == 3 :
                                    x => x.RiskDetail.RiskId == fileuploadfilter.RiskDetailId && x.IsActive != false && x.FileCategoryId != null && x.FileCategoryId != 3;
            }
            else
            {
                expression = !fileuploadfilter.isPolicy ?
                      x => x.RiskDetailId == fileuploadfilter.RiskDetailId && x.IsActive != false && x.FileCategoryId != null && (x.Description.ToLower().Contains(fileuploadfilter.Keyword) || x.FileCategory.Description.ToLower().Contains(fileuploadfilter.Keyword) || (x.CreatedDate >= startdtKeyword && x.CreatedDate <= enddtKeyword)) :
                      fileuploadfilter.isBilling ?
                          x => x.RiskDetail.RiskId == fileuploadfilter.RiskDetailId && x.IsActive != false && x.FileCategoryId != null && x.FileCategoryId == 3 :
                          x => x.RiskDetail.RiskId == fileuploadfilter.RiskDetailId && x.IsActive != false && x.FileCategoryId != null && x.FileCategoryId != 3 && (x.Description.Contains(fileuploadfilter.Keyword) || x.FileCategory.Description.Contains(fileuploadfilter.Keyword) || (x.CreatedDate >= startdtKeyword && x.CreatedDate <= enddtKeyword));

              

            }
            Expression<Func<FileUploadDocument, object>> selector;
            selector = x => x.CreatedDate;

            //bool willSort = fileuploadfilter.isPolicy && fileuploadfilter.isBilling ? false : true; 

            #region Sorting

            selector = fileuploadfilter.SortField switch
            {
                "CreatedDate" => x => x.CreatedDate,
                "Description" => x => x.Description,
                "Category" => x => x.FileCategory.Description,
                _ => x => x.CreatedDate,
            };
           
            #endregion

            var fileUploadDocumentList = new List<FileUploadDocument>();
            var (result, paginationDetails) = await _FileUploadRepository.GetWithPagingAsync(expression, selector, fileuploadfilter.PageNumber, fileuploadfilter.PageSize, fileuploadfilter.Order);
                
            foreach (var item in result.ToList())
            {
                  
                    var bind = await _bindingRequirementsRepository.FindAsync(x => x.Id == item.tableRefId);
                    if (bind != null && bind.BindStatusId == BindStatus.Pending.Id)
                        continue;
                   
                    if (!CheckIsAllowedEditDeleteAsync(item.Id).Result)
                        item.IsSystemGenerated = true;
                    

                fileUploadDocumentList.Add(_mapper.Map<FileUploadDocument>(item));
            }

            
            return new FileUploadPagingDTO()
            {
                PaginationDetails = paginationDetails,
                FileUploadDocuments = fileUploadDocumentList

            };
            
            
        }

        public async Task<FileUploadPagingDTO> GetByRiskIdAllAsync(FileUploadDocFilterDTO fileuploadfilter)
        {
            DateTime d;

            bool chValidity = DateTime.TryParseExact(
            fileuploadfilter.Keyword,
            "MM/dd/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out d);

            DateTime? startdtKeyword = chValidity ? Convert.ToDateTime(fileuploadfilter.Keyword + " 00:00") : null;
            DateTime? enddtKeyword = chValidity ? Convert.ToDateTime(fileuploadfilter.Keyword + " 12:59") : null;

            Expression<Func<FileUploadDocument, bool>> expression;
            if (fileuploadfilter.Keyword == null || fileuploadfilter.Keyword.Length <= 0)
            {
                expression = !fileuploadfilter.isPolicy ?
                                x => x.RiskDetailId == fileuploadfilter.RiskDetailId && x.IsActive != false && x.FileCategoryId != null :
                                fileuploadfilter.isBilling ?
                                    x => x.RiskDetail.RiskId == fileuploadfilter.RiskDetailId && x.IsActive != false && x.FileCategoryId != null && x.FileCategoryId == 3 :
                                    x => x.RiskDetail.RiskId == fileuploadfilter.RiskDetailId && x.IsActive != false && x.FileCategoryId != null && x.FileCategoryId != 3;
            }
            else
            {
                expression = !fileuploadfilter.isPolicy ?
                      x => x.RiskDetailId == fileuploadfilter.RiskDetailId && x.IsActive != false && x.FileCategoryId != null && (x.Description.ToLower().Contains(fileuploadfilter.Keyword) || x.FileCategory.Description.ToLower().Contains(fileuploadfilter.Keyword) || (x.CreatedDate >= startdtKeyword && x.CreatedDate <= enddtKeyword)) :
                      fileuploadfilter.isBilling ?
                          x => x.RiskDetail.RiskId == fileuploadfilter.RiskDetailId && x.IsActive != false && x.FileCategoryId != null && x.FileCategoryId == 3 :
                          x => x.RiskDetail.RiskId == fileuploadfilter.RiskDetailId && x.IsActive != false && x.FileCategoryId != null && x.FileCategoryId != 3 && (x.Description.Contains(fileuploadfilter.Keyword) || x.FileCategory.Description.Contains(fileuploadfilter.Keyword) || (x.CreatedDate >= startdtKeyword && x.CreatedDate <= enddtKeyword));



            }

            var fileUploadDocumentList = new List<FileUploadDocument>();
            var result = await _FileUploadRepository.GetAllIncludeAsync(expression);

            foreach (var item in result.ToList())
            {

                var bind = await _bindingRequirementsRepository.FindAsync(x => x.Id == item.tableRefId);
                if (bind != null && bind.BindStatusId == BindStatus.Pending.Id)
                    continue;

                if (!CheckIsAllowedEditDeleteAsync(item.Id).Result)
                    item.IsSystemGenerated = true;


                fileUploadDocumentList.Add(_mapper.Map<FileUploadDocument>(item));
            }


            return new FileUploadPagingDTO()
            {
                FileUploadDocuments = fileUploadDocumentList

            };


        }

        public async Task<List<FileUploadDocumentDTO>> DownloadAllDocumentsAsync(FileUploadDocFilterDTO fileuploadfilter)
        {
            int ftpCount = 0;
            List<FileUploadDocument> attachmentList = new List<FileUploadDocument>();
            List<FileUploadDocumentDTO> finalattachmentList = new List<FileUploadDocumentDTO>();
            if (fileuploadfilter.isPolicy)
            {
                fileuploadfilter.isBilling = false;
                var policyDocuments = await GetByRiskIdAllAsync(fileuploadfilter);
                attachmentList.AddRange(policyDocuments.FileUploadDocuments);
                fileuploadfilter.isBilling = true;
                var billingDocuments = await GetByRiskIdAllAsync(fileuploadfilter);
                attachmentList.AddRange(billingDocuments.FileUploadDocuments);
            }
            else
            {
                var res = await GetByRiskIdAllAsync(fileuploadfilter);
                attachmentList.AddRange(res.FileUploadDocuments);
            }

            foreach (var data in attachmentList)
            {
                try
                {
                    if (data.IsUploaded == false) continue;
                    var fileName = Path.GetFileName(data.FilePath);
                    finalattachmentList.Add(new FileUploadDocumentDTO() { 
                        //FileName = $"{data.Description} - {data.FileName}", 
                        FileName = fileName,
                        FilePath = data.FilePath,
                    });
                }
                catch (Exception ex)
                {
                    _logger.Error("Error Message: Stream File Error. " + ex.Message, JsonConvert.SerializeObject(data));
                }
            }

            return finalattachmentList;
        }

        private bool CreateTempFolder(string folderName)
        {
            try
            {
                if (!(Directory.Exists(folderName))) 
                    Directory.CreateDirectory(folderName);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return false;
            }
        }

        private void StreamFile(string fileUrl, string folderName, string fileName)
        {
            //fileUrl = FileHelperUtility.EncodeFilePath(fileUrl);
            string fileStream = folderName + '/' + fileName;
            WebClient client = new WebClient();
            client.DownloadFile(_SASURLGeneratorService.GenerateSharedAccessSignatureURL(fileUrl),
                fileStream);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(delegate (object sender, DownloadProgressChangedEventArgs e)
            {
                Console.WriteLine("StreamFile:" + e.ProgressPercentage.ToString());
            });

            client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler
             (delegate (object sender, System.ComponentModel.AsyncCompletedEventArgs e)
             {
                 if (e.Error == null && !e.Cancelled)
                 {
                     Console.WriteLine("Stream completed!");
                 }
             });

            client.Dispose();
        }

        private void ZipFolderFile(string folderName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string folderDir = Path.Combine(currentDirectory, folderName);
            string zipFileName = folderName + ".zip";
            if (File.Exists(zipFileName))
                File.Delete(zipFileName);

            ZipFile.CreateFromDirectory(folderDir, Path.Combine(currentDirectory, zipFileName), CompressionLevel.Optimal, false);
        }

        public async Task<string> SendToAzureStorage(string filePath)
        {
            string uploadResult = null;
            try
            {
                using (FileStream fs = File.Open(filePath?.Trim(), FileMode.Open, FileAccess.ReadWrite))
                {
                    uploadResult = await _storageService.Upload(fs, filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "api/DownloadAllDocuments/SendToAzureStorage");
            }

            return uploadResult;
        }

        private void FolderDelete(string folderName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string folderDir = Path.Combine(currentDirectory, folderName);
            Directory.Delete(folderDir, true);
        }

        private void DeleteZipFile(string folderName)
        {
            string zipFileName = folderName + ".zip";
            if (File.Exists(zipFileName))
                File.Delete(zipFileName);
        }
    }
}