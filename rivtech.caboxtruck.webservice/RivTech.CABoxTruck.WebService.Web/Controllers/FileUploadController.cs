using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize] // this not advisable but for now disabled to allow form upload file
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadDocumentService _service;
        private readonly IStorageService _storageService;
        private readonly IRiskService _riskService;

        public FileUploadController(IFileUploadDocumentService service, IStorageService storageService, IRiskService riskService)
        {
            _service = service;
            _storageService = storageService;
            _riskService = riskService;
        }

        [HttpPost]
        [Route("UploadAzure")]
        public async Task<IActionResult> uploadAzure()
        {
            try
            {
                var postedFile = Request.Form.Files[0];
                string States = Request.Form["states"];

                if (postedFile.Length < 1)
                {
                    return BadRequest("No file found from request.");
                }

                var result = await _service.uploadAzure(postedFile.OpenReadStream());

                return Ok(result);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetFileUploadByRefId/{id}")]
        [Produces(typeof(FileUploadDocumentDTO))]
        public async Task<IActionResult> GetByRefId([FromRoute] Guid id)
        {
            try
            {
                var res = await _service.GetByRefIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetFileUploadByRiskDetailId/{id}")]
        [Produces(typeof(FileUploadDocumentDTO))]
        public async Task<IActionResult> GetByRiskDetailId([FromRoute] Guid id)
        {
            try
            {
                var res = await _service.GetByRiskDetailIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetFileUploadByRiskId/{id}")]
        [Produces(typeof(FileUploadDocumentDTO))]
        public async Task<IActionResult> GetByRiskId([FromRoute] Guid id)
        {
            try
            {
                var res = await _service.GetByRiskIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        //[HttpGet("GetDocumentsByRiskId/{id}")]
        [HttpPost("Documentlist")]
        //[Produces(typeof(FileUploadPagingDTO))]
        [Produces("application/json")]
        public async Task<ActionResult<FileUploadPagingDTO>> GetTaskListAsync([FromBody] FileUploadDocFilterDTO filterDTO)
        {
            try
            {
                var res = await _service.GetByRiskIdAsync(filterDTO);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpGet("CheckFileUploadId/{id}")]
        public async Task<IActionResult> CheckById([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _service.CheckIsUploadedByIdAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("CheckIsAllowedToEditDelete/{id}")]
        public async Task<IActionResult> CheckIsAllowedToEditDelete([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _service.CheckIsAllowedEditDeleteAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpPost("User")]
        [Authorize]
        public async Task<IActionResult> Post()
        {
            try
            {
                Request.Form.TryGetValue("riskDocument", out var riskDocumentValue);
                var riskDocument = JsonConvert.DeserializeObject<FileUploadDocumentDTO>(riskDocumentValue);

                if (Request.Form.Files.Count > 0)
                {
                    var fileNames = new List<string>();
                    foreach(var file in Request.Form.Files)
                    {
                        fileNames.Add(file.FileName);
                    }

                    if (await _service.IsDocumentDuplicateFileNameAsync(riskDocument.RiskDetailId.Value, fileNames, riskDocument.Id))
                        return Ok(new { Message = "Duplicate filename is not allowed" });

                    if (Request.Form.Files.Count > 1)
                    {
                        // Create ZIP
                        var byteFiles = _service.CreateZipFile(Request.Form.Files);
                        var fileName = $"{DateTime.Now:MMddyyyyhhmmssff}.zip";
                        riskDocument.FileName = string.Join("\r\n", Request.Form.Files.ToList().Select(x => x.FileName));
                        var relativePath = $"{DateTime.Now:yyyy-MM}/{riskDocument.tableRefId}/{fileName}";
                        var uploadResult = await _storageService.Upload(byteFiles, relativePath, _FOLDER_PATH: Request.Form["states"]);
                        riskDocument.FilePath = uploadResult;
                    } else
                    {
                        var postedFile = Request.Form.Files[0];
                        riskDocument.FileName = postedFile.FileName;
                        var relativePath = $"{DateTime.Now:yyyy-MM}/{riskDocument.tableRefId}/{postedFile.FileName}";
                        var uploadResult = await _storageService.Upload(postedFile, relativePath, _FOLDER_PATH: Request.Form["states"]);
                        riskDocument.FilePath = uploadResult;
                    }
                    riskDocument.CreatedDate = DateTime.Now;
                }
                riskDocument.IsUploaded = true;
                riskDocument.IsSystemGenerated = false;
                riskDocument.IsUserUploaded = true;

                await _service.AddUpdateAsync(riskDocument);

                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("QuoteInfo")]
        public async Task<IActionResult> Post(FileUploadDocumentDTO resource)
        {
            await _service.AddUpdateAsync(resource);
            await _riskService.UpdateRiskStatusOnlyAsync(resource.tableRefId.Value, RiskDetailStatus.Quoted);
            return Ok();
        }

        [HttpPost("AccountsReceivable")]
        public async Task<IActionResult> AccountsReceivable()
        {
            var postedFile = Request.Form.Files[0];
            var relativePath = $"AccountsReceivable_{DateTime.Now:MM_dd_yyyy}.pdf";
            var uploadResult = await _storageService.UploadAR(postedFile, relativePath);
            return Ok(uploadResult);
        }

        [HttpPost("PaymentPosting")]
        public async Task<IActionResult> PaymentPosting(string fromDate, string toDate)
        {
            var postedFile = Request.Form.Files[0];
            var relativePath = postedFile.FileName;
            var uploadResult = await _storageService.UploadAR(postedFile, relativePath);
            return Ok(uploadResult);
        }

        [HttpPost("BrokerCommission")]
        public async Task<IActionResult> BrokerCommission(string fromDate, string toDate)
        {
            var postedFile = Request.Form.Files[0];
            var relativePath = postedFile.FileName;
            var uploadResult = await _storageService.UploadAR(postedFile, relativePath);
            return Ok(uploadResult);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                if (id == Guid.Empty) return BadRequest();
                await _service.RemoveAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Download")]
        public async Task<IActionResult> Download([FromBody] List<Guid> documentIds)
        {
            var outputStream = await _storageService.Download(await _service.GetByIdsAsync(documentIds));
            return File(outputStream, "application/octet-stream", "BT Documents.zip");
        }

        [HttpPost]
        [Route("DownloadAll")]
        public async Task<IActionResult> DownloadAll([FromBody] FileUploadDocFilterDTO filterDTO) // 
        {
            var outputStream = await _storageService.Download(await _service.DownloadAllDocumentsAsync(filterDTO));
            var contentBytes = outputStream.ToArray();
            return new FileContentResult(contentBytes, "application/vnd.ms-excel.sheet.binary.macroEnabled.12");
        }
    }
}
