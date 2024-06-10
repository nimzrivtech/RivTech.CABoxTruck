using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize] // this not advisable but for now disabled to allow form upload file
    [ApiController]
    public class BindingController : ControllerBase
    {
        private readonly IBindingService _service;
        private readonly IStorageService _storageService;
        private readonly ISLNumberService _SLNumberService;
        private readonly IFileUploadDocumentService _fileUploadDocumentService;
        private readonly IApplogService _applogService;


        public BindingController(IBindingService service,
            IStorageService storageService,
            ISLNumberService sLNumberService,
            IFileUploadDocumentService fileUploadDocumentService,
            IApplogService applogService
        )
        {
            _service = service;
            _storageService = storageService;
            _SLNumberService = sLNumberService;
            _fileUploadDocumentService = fileUploadDocumentService;
            _applogService = applogService;
        }

      
        #region Binding
        [HttpPost("AddUpdate")]
        [Produces(typeof(BindingDTO))]
        public async Task<IActionResult> AddUpdate([FromBody] BindingDTO resource)
        {
            try
            {
                var temp = JsonConvert.SerializeObject(resource);

                await _applogService.LogDocumentGeneration($"DocumentUpload:{resource.RiskId}", "AddUpdate", temp);
                return Ok(await _service.AddUpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("getSLADefault")]
        [Produces(typeof(SLNumberDTO))]
        public async Task<IActionResult> getSLADefault(Int16? creditId, string state)
        {
            try
            {
                return Ok(await _SLNumberService.GetByCreditOfficeIdStateAsync(creditId, state));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
        #endregion Binding

        #region Binding Requirements
        [HttpGet("Requirements/{id}")]
        [Produces(typeof(BindingRequirementsDTO))]
        public async Task<IActionResult> AddUpdateRequirements([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _service.GetByRiskDetailIdAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Requirements/AddUpdate")]
        [Produces(typeof(BindingRequirementsDTO))]
        public async Task<IActionResult> AddUpdateRequirements([FromBody] BindingRequirementsDTO resource)
        {
            try
            {
                var temp = JsonConvert.SerializeObject(resource);

                await _applogService.LogDocumentGeneration($"DocumentUpload:{resource.RiskDetailId}", "Requirements/AddUpdate", temp);
                return Ok(await _service.AddUpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Requirements/AddUpdateWithFile")]
        //[Produces(typeof(BindingRequirementsDTO))]
        public async Task<IActionResult> AddUpdateRequirementsWithFile()
        {
            try
            {
                var resource = JsonConvert.DeserializeObject<BindingRequirementsDTO>(Request.Form["data"]);
                resource.CreatedDate = DateTime.Now;
                if (resource.FileUploads != null && resource.FileUploads.Any())
                {
                    try
                    {
                        var fileNames = resource.FileUploads.Select(x => x.FileName).ToList();
                        if (await _fileUploadDocumentService.IsBindQuoteDuplicateFileNameAsync(resource.RiskDetailId.Value, fileNames, resource.Id))
                            return new JsonResult(new { data = resource, status = true, message = "Duplicate filename is not allowed" }, new JsonSerializerSettings());

                        if (Request.Form.Files.Count > 1)
                        {
                            // Create ZIP
                            var byteFiles = _fileUploadDocumentService.CreateZipFile(Request.Form.Files);
                            resource = await _service.AzureBlogSave(byteFiles, Request.Form["states"], "bind-req", resource);
                        } 
                        else
                        {
                            resource = await _service.AzureBlogSave(Request.Form.Files, Request.Form["states"], "bind-req", resource);
                        }
                    }
                    catch (Exception)
                    {
                        //if failed saving the file. 
                        resource.FileUploads.ToList().ForEach(f => {
                            f.CreatedDate = DateTime.Now;
                            f.IsUploaded = false; });

                        resource = await _service.AddUpdateAsync(resource);
                        return new JsonResult(new { data = resource, status = true, message = "A record has been saved but here was an error trying to upload the file. Please try again." }, new JsonSerializerSettings());
                    }
                }

               resource = await _service.AddUpdateAsync(resource);

                var documentLink = _fileUploadDocumentService.GetByRefIdAsync(resource.Id.Value).Result.FirstOrDefault();
                if (documentLink != null)
                    resource.FilePath = documentLink.FilePath;

                var temp = JsonConvert.SerializeObject(resource);

                await _applogService.LogDocumentGeneration($"BindRequirementUpload:{resource.RiskDetailId}", "AddUpdateAsync", temp);
                return new JsonResult(new { data = resource, status = true, message = "" }, new JsonSerializerSettings());
                //return Ok(await _service.AddUpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("Requirements/Delete/{id}")]
        [Produces(typeof(BindingRequirementsDTO))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty) return BadRequest();

                var bindingRequirementsDTO = await _service.GetByIdAsync(id);
                var documentLink = await _fileUploadDocumentService.GetByRefIdAsync(id);

                if (documentLink.Any())
                {
                    bindingRequirementsDTO.FileUploads = new List<FileUploadDocumentDTO>();

                    bindingRequirementsDTO.FileUploads.AddRange(documentLink);
                }

                await _service.RemoveAsync(id);

                var temp = JsonConvert.SerializeObject(bindingRequirementsDTO);

                await _applogService.LogDocumentGeneration($"BindRequirementDelete:{bindingRequirementsDTO.RiskDetailId}", "RemoveAsync", temp);

                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
        #endregion Binding Requirements

        #region QuoteConditions

        [Authorize]
        [HttpPost("Quote/AddUpdateQuoteWithFile")]
        public async Task<IActionResult> AddUpdateQuoteWithFile()
        {
            try
            {
                var resource = JsonConvert.DeserializeObject<QuoteConditionsDTO>(Request.Form["data"]);
                resource.CreatedDate = DateTime.Now;
                if (resource.FileUploads != null && resource.FileUploads.Any())
                {
                    try
                    {
                        var fileNames = resource.FileUploads.Select(x => x.FileName).ToList();
                        if (await _fileUploadDocumentService.IsBindQuoteDuplicateFileNameAsync(resource.RiskDetailId.Value, fileNames, resource.Id))
                            return new JsonResult(new { data = resource, status = true, message = "Duplicate filename is not allowed" }, new JsonSerializerSettings());

                        if (Request.Form.Files.Count > 1)
                        {
                            // Create ZIP
                            var byteFiles = _fileUploadDocumentService.CreateZipFile(Request.Form.Files);
                            resource = await _service.AzureBlogSave<QuoteConditionsDTO>(byteFiles, Request.Form["states"], "quote-cond", resource);
                        }
                        else
                        {
                            resource = await _service.AzureBlogSave<QuoteConditionsDTO>(Request.Form.Files, Request.Form["states"], "quote-cond", resource);
                        }
                    }
                    catch (Exception)
                    {
                        //if failed saving the file. 
                        resource.FileUploads.ToList().ForEach(f => {
                            f.CreatedDate = DateTime.Now;
                            f.IsUploaded = false;
                        });

                        resource = await _service.AddUpdateAsync(resource);
                        return new JsonResult(new { data = resource, status = true, message = "A record has been saved but here was an error trying to upload the file. Please try again." }, new JsonSerializerSettings());
                    }
                }

                resource = await _service.AddUpdateAsync(resource);

                if (!resource.Id.HasValue)
                    return new JsonResult(new { data = resource, status = true, message = "" }, new JsonSerializerSettings());

                var documentLink = _fileUploadDocumentService.GetByRefIdAsync(resource.Id.Value).Result.FirstOrDefault();
                if (documentLink != null)
                    resource.FilePath = documentLink.FilePath;

                var temp = JsonConvert.SerializeObject(resource);

                await _applogService.LogDocumentGeneration($"QuoteConditionUpload:{resource.RiskDetailId}", "AddUpdateAsync", temp);

                return new JsonResult(new { data = resource, status = true, message = "" }, new JsonSerializerSettings());
                //return Ok(await _service.AddUpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("Quote/DeleteQuote/{id}")]
        [Produces(typeof(QuoteConditionsDTO))]
        public async Task<IActionResult> DeleteQuote([FromRoute] Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty) return BadRequest();

                var quoteConditionDTO = await _service.GetQuoteByIdAsync(id);
                var documentLink = await _fileUploadDocumentService.GetByRefIdAsync(id);

                if (documentLink.Any())
                {
                    quoteConditionDTO.FileUploads = new List<FileUploadDocumentDTO>();

                    quoteConditionDTO.FileUploads.AddRange(documentLink);
                }

                await _service.RemoveQuoteAsync(id);

                var temp = JsonConvert.SerializeObject(quoteConditionDTO);

                await _applogService.LogDocumentGeneration($"QuoteConditionDelete:{quoteConditionDTO.RiskDetailId}", "RemoveAsync", temp);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpGet("Quote/hasRecord/{id}")]
        [Produces(typeof(SLNumberDTO))]
        public async Task<IActionResult> hasRecord([FromRoute] Guid id)
        {
            try
            {
                return Ok(_service.GetQuoteByRiskDetailIdAsync(id).Result.Any());
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Quote/IsQuoteConditionOptionExist/{id}")]
        public async Task<IActionResult> IsQuoteConditionOptionExist(Guid id, [FromBody] int[] quoteConditionOptionIds)
        {
            try
            {
                return Ok(await _service.GetQuoteConditionOptionIsExist(id, quoteConditionOptionIds));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Quote/SetQuoteConditionOptions")]
        public async Task<IActionResult> SetQuoteConditionOptions([FromBody] QuoteConditionRequestDTO quoteConditionRequest)
        {
            try
            {
                return Ok(await _service.SetQuoteConditionOptions(quoteConditionRequest));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        #endregion
    }
}
