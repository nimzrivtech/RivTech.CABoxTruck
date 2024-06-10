using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Renci.SshNet;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Report.Filters;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Report;
using RivTech.CABoxTruck.WebService.Service.Services.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services
{
    public class FTPClientServerSevice : IFTPClientServerSevice
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILog4NetService _logger;
        private readonly IApplogService _applogService;
        private readonly IFTPDocumentRepository _FTPDocumentRepository;
        private readonly IFTPDocumentTemporaryRepository _FTPDocumentTemporaryRepository;
        private readonly IFTPDocumentFilesCountRepository _FTPDocumentFilesCountRepository;
        private readonly IStorageService _storageService;
        private readonly IReportService _reportService;
        private readonly ISASURLGeneratorService _SASURLGeneratorService;

        public FTPClientServerSevice(IConfiguration configuration, 
            IMapper mapper, 
            ILog4NetService logger,
            IApplogService applogService,
            IStorageService storageService,
            IReportService reportService,
            IFTPDocumentRepository FTPDocumentRepository,
            IFTPDocumentTemporaryRepository FTPDocumentTemporaryRepository,
            IFTPDocumentFilesCountRepository FTPDocumentFilesCountRepository,
            ISASURLGeneratorService SASURLGeneratorService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _applogService = applogService;
            _storageService = storageService;
            _reportService = reportService;
            _FTPDocumentRepository = FTPDocumentRepository;
            _FTPDocumentTemporaryRepository = FTPDocumentTemporaryRepository;
            _FTPDocumentFilesCountRepository = FTPDocumentFilesCountRepository;
            _SASURLGeneratorService = SASURLGeneratorService;
        }

        #region Private
        private FTPClientServerDTO GetFTPDetails(string section)
        {
            var ftpSection = _configuration.GetSection($"FTPClientServer:{section}");

            return new FTPClientServerDTO()
            {
                BaseUrl = ftpSection.GetValue<string>("BaseUrl"),
                Port = ftpSection.GetValue<int>("Port"),
                IsSsh = ftpSection.GetValue<bool>("IsSsh"),
                Username = ftpSection.GetValue<string>("Username"),
                Password = ftpSection.GetValue<string>("Password"),
                Directory = ftpSection.GetValue<string>("Directory")
            };
        }

        private int GetFTPClientType(bool isSsh)
        {
            if (isSsh)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        private async Task<bool> UploadSshClientType(FTPClientServerDTO ftpClientServerDTO, string filePath)
        {
            bool isUploaded = false;
            using (SftpClient sftp = new SftpClient(ftpClientServerDTO.BaseUrl, ftpClientServerDTO.Port, ftpClientServerDTO.Username, ftpClientServerDTO.Password))

                try
                {
                    sftp.Connect();
                    string ftpDir = ftpClientServerDTO.Directory.ToString();
                    if (!string.IsNullOrWhiteSpace(ftpDir))
                        sftp.ChangeDirectory(ftpDir);
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        sftp.UploadFile(fs, filePath);
                        sftp.Disconnect();
                    }
                    isUploaded = true;

                    if (isUploaded)
                    {
                        await _applogService.Save("Upload To FTP Successful", filePath?.Trim());
                    }
                    else
                    {
                        _logger.Error("Upload To FTP unsuccessful", filePath?.Trim());
                    }

                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "api/FTPClientServer/ftpZipSend");

                    isUploaded = false;
                }

            return await Task.FromResult(isUploaded);
        }

        private async Task<bool> UploadDefaultClientType(FTPClientServerDTO ftpClientServerDTO, string filePath)
        {
            bool isUploaded = false;

            string Url = $"ftp://{ftpClientServerDTO.BaseUrl}:{ftpClientServerDTO.Port}/" + ftpClientServerDTO.Directory + "/" + filePath;
            try
            {
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(ftpClientServerDTO.Username, ftpClientServerDTO.Password);
                    //client.BaseAddress = Url;
                    client.UploadFile(Url, WebRequestMethods.Ftp.UploadFile, filePath);
                    isUploaded = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "api/FTPClientServer/ftpZipSend");

                isUploaded = false;
            }

            return await Task.FromResult(isUploaded);
        }

        private async Task<bool> Upload(FTPClientServerDTO ftpClientServerDTO, int clientType, string filePath)
        {
            bool isUploaded;
            isUploaded = clientType == 1 ? await UploadDefaultClientType(ftpClientServerDTO, filePath) :
                                            await UploadSshClientType(ftpClientServerDTO, filePath);

            return isUploaded;
        }

        #endregion
        /// <summary>
        /// Includes Report Generation.
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public async Task<bool> SendZipToFTPServerAsync(string batchId = null)
        {
            var collectionData = new List<FTPDocumentTemporary>();

            string folderName = FileHelperUtility.ScrubbedName($"CABT_{DateTime.UtcNow:MMddyyyyHHmmss_ffff}");

            int ftpCount = 0;
            int certifiedCount = 0;
            int invoiceCount = 0;
            int regularCount = 0;

            bool isUploaded = false;

            List<string> ValidSources = new List<string>()
            {
                GenerateDocumentSource.InstallmentInvoice,
                GenerateDocumentSource.Reinstatement,
                GenerateDocumentSource.Cancellation,
                GenerateDocumentSource.NonPayNoticePendingCancellation,
                GenerateDocumentSource.RescindNonPayNoticePendingCancellation,
                GenerateDocumentSource.DepositInvoice
            };

            IEnumerable<FTPDocumentTemporary> ftpDocuments = null;

            if (!string.IsNullOrEmpty(batchId))
            {
                DateTime dateTime;
                DateTime.TryParseExact(batchId, "MMddyyyy", null, DateTimeStyles.None, out dateTime);
                ftpDocuments = (await _FTPDocumentTemporaryRepository.GetAsync(a => (a.BatchId == batchId) // || a.CreatedDate < dateTime
                    && ValidSources.Contains(a.Source))).OrderBy(b => b.CreatedDate);
            }
            else
            {
                ftpDocuments = (await _FTPDocumentTemporaryRepository.GetAsync(a => ValidSources.Contains(a.Source))).OrderBy(b => b.CreatedDate);
            }

            if (!CreateTempFolder(folderName))
            {
                throw new Exception("Error creating Folder. BatchId: " + batchId);
            }

            foreach (var data in ftpDocuments)
            {
                try
                {
                    var subfolder = GetSubFolder(data);
                    StreamFile(data.FilePath, folderName, FileHelperUtility.MakeFileNameUnique(FileHelperUtility.ScrubbedName(data.FileName)), FileHelperUtility.ScrubbedName(subfolder));

                    switch (subfolder)
                    {
                        case "Certified": certifiedCount++; break;
                        case "Invoice": invoiceCount++; break;
                        case "Regular": regularCount++; break;
                        default: throw new Exception("Unknown subfolder.");
                    }

                    collectionData.Add(data);
                    ftpCount++;
                }
                catch (Exception ex)
                {
                    _logger.Error("Error Message: Stream File Error. " + ex.Message, JsonConvert.SerializeObject(data));
                }
            }

            try
            {
                if (ftpCount >= 1)
                {
                    //Add POM Report
                    var certifiedFolderPath = folderName + @"/Certified";
                    await GeneratePOMReport(certifiedFolderPath);

                    ZipFolderFile(folderName);

                    var fileSent = await SendToCovenirServer(folderName + ".zip");

                    if (fileSent)
                    {
                        FolderDelete(folderName);
                        DeleteZipFile(folderName);

                        foreach (var removeData in collectionData)
                        {
                            _FTPDocumentTemporaryRepository.Remove(removeData);
                        }

                        isUploaded = await _FTPDocumentRepository.UpdateByBatchId(collectionData);
                        isUploaded = fileSent;

                        await AddFTPFilesCount(new FTPDocumentFilesCountDTO
                        {
                            Id = Guid.NewGuid(),
                            RegularCount = regularCount,
                            InvoiceCount = invoiceCount,
                            CertifiedCount = certifiedCount,
                            FTPSentDate = DateTime.Now.Date,
                        });
                    }
                    else
                    {
                        FolderDelete(folderName);
                        DeleteZipFile(folderName);

                        isUploaded = false;

                        throw new Exception("Error sending to ftp. BatchId: " + batchId);
                    }
                }
                else
                {
                    FolderDelete(folderName);

                    isUploaded = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw ex;
            }

            return isUploaded;
        }

        private string GetSubFolder(FTPDocumentTemporary fTPDocument)
        {
            const string _certified = @"Certified";
            const string _invoiceFolder = @"Invoice";
            const string _regular = @"Regular";


            List<string> invoiceFolderSources = new List<string>()
            {
                GenerateDocumentSource.InstallmentInvoice,
                GenerateDocumentSource.DepositInvoice
            };

            List<string> certifiedFolderSources = new List<string>()
            {
                GenerateDocumentSource.Cancellation,
                GenerateDocumentSource.NonPayNoticePendingCancellation
            };

            if (invoiceFolderSources.Contains(fTPDocument.Source))
            {
                return _invoiceFolder;
            }
            else if (certifiedFolderSources.Contains(fTPDocument.Source))
            {
                return _certified;
            }
            else
            {
                return _regular;
            }
        }

        public async Task SaveFTPDocument(Guid riskId, FileUploadDocument fileUploadDocument)
        {
            var source = "";
            if ((fileUploadDocument.Description?.Contains(GenerateDocumentDescription.Cancellation) ?? false) 
                || (fileUploadDocument.Description?.Contains(GenerateDocumentDescription.FinalCancellation) ?? false))
                source = GenerateDocumentSource.Cancellation;

            if (string.IsNullOrEmpty(source))
                return;

            await AddToFTP(new FTPDocumentDTO()
            {
                RiskId = riskId,
                RiskDetailId = fileUploadDocument.RiskDetailId,
                Description = fileUploadDocument.Description,
                FileName = fileUploadDocument.FileName,
                FilePath = fileUploadDocument.FilePath,
                IsUploaded = false,
                Source = source,
                BatchId = DateTime.Now.ToString("MMddyyyy"),
                IsCompiled = false,
                Category = "MISC"
            });
        }

        public async Task<bool> SendToCovenirServer(string filePath)
        {
            string configSection = "CABoxtruck";
            bool isUploaded = false;
            int clientType = 0;

            try
            {
                var ftpClientServerDTO = GetFTPDetails(configSection);
                clientType = GetFTPClientType(ftpClientServerDTO.IsSsh);

                isUploaded = await Upload(ftpClientServerDTO, clientType, filePath);


                if (isUploaded)
                {
                    try
                    {
                        using (FileStream fs = File.Open(filePath?.Trim(), FileMode.Open, FileAccess.ReadWrite))
                        {
                            var pathResult = await _storageService.Upload(fs, filePath, _FOLDER_PATH: "Covenir");
                        }
                        await _applogService.Save("Upload backup Successful", filePath?.Trim());

                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Upload backup failed", filePath?.Trim());
                    }
                }
                else
                {
                    _logger.Error("Upload To FTP Unsuccessful", filePath?.Trim());
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "api/FTPClientServer/ftpZipSend");
            }

            return isUploaded;
        }

        private async Task<bool> AddToFTP(FTPDocumentDTO document)
        {
            try
            {
                await _FTPDocumentRepository.AddAsync(_mapper.Map<FTPDocument>(document));
                await _FTPDocumentTemporaryRepository.AddAsync(_mapper.Map<FTPDocumentTemporary>(document));

                await _FTPDocumentRepository.SaveChangesAsync();
                await _FTPDocumentTemporaryRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private async Task<bool> AddFTPFilesCount(FTPDocumentFilesCountDTO document)
        {
            try
            {
                await _FTPDocumentFilesCountRepository.AddAsync(_mapper.Map<FTPDocumentFilesCount>(document));
                await _FTPDocumentFilesCountRepository.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void DeleteZipFile(string folderName)
        {
            string zipFileName = folderName + ".zip";
            if (File.Exists(zipFileName))
                File.Delete(zipFileName);
        }

        private void ZipFolderFile(string folderName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string folderDir = Path.Combine(currentDirectory, folderName);
            string zipFileName = folderName + ".zip";
            if (File.Exists(zipFileName))
                File.Delete(zipFileName);

            ZipFile.CreateFromDirectory(folderDir, Path.Combine(currentDirectory, zipFileName), CompressionLevel.Optimal, false);
            //Directory.Delete(folderDir, true);
        }

        private void FolderDelete(string folderName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string folderDir = Path.Combine(currentDirectory, folderName);
            Directory.Delete(folderDir, true);
        }

        private void StreamFile(string fileUrl, string folderName, string fileName, string subFolderName = null)
        {
            //fileUrl = FileHelperUtility.EncodeFilePath(fileUrl);
            string fileStream = subFolderName == null ? folderName + '/' + fileName : folderName + $@"/{subFolderName}/" + fileName;
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

        private bool CreateTempFolder(string folderName)
        {
            const string _certified = @"/Certified";
            const string _invoiceFolder = @"/Invoice";
            const string _regular = @"/Regular";

            try
            {
                if (!(Directory.Exists(folderName)))
                {
                    Directory.CreateDirectory(folderName);
                    if (!(Directory.Exists(folderName + _invoiceFolder))) Directory.CreateDirectory(folderName + _invoiceFolder);
                    if (!(Directory.Exists(folderName + _certified))) Directory.CreateDirectory(folderName + _certified);
                    if (!(Directory.Exists(folderName + _regular))) Directory.CreateDirectory(folderName + _regular);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return false;
            }
        }

        private async Task GeneratePOMReport(string filePath)
        {
            var memoryStream = new MemoryStream();

            var reportManagementFilter = new ReportManagementFilter()
            {
                ReportType = "POMReport",
                ReportFileName = $"CABT - POM {DateTime.Now.ToString("MMddyyyy")}"
            };

            try
            {
                var stream = await _reportService.GetReportStream(reportManagementFilter);

                using (var outputFileStream = File.Create($"{filePath}//{reportManagementFilter.ReportFileName}.xlsx"))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(outputFileStream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating file: " + ex.Message);
            }
        }
    }
}