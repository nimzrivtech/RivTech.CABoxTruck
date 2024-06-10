using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly IFormsService _formsService;
        private readonly IStorageService _storageService;
        private readonly IRiskDetailRepository _riskDetail;

        public FormsController(IFormsService formsService, IStorageService storageService, IRiskDetailRepository riskDetail)
        {
            _formsService = formsService;
            _storageService = storageService;
            _riskDetail = riskDetail;
        }

        [HttpGet("{id}")]
        [Produces(typeof(List<ClaimsHistoryDTO>))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _formsService.GetByRiskIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetByForm/{riskDetailId}")]
        public async Task<IActionResult> GetByForm([FromRoute] Guid riskDetailId, [FromQuery] string formId)
        {
            try
            {
                var forms = await _formsService.GetByRiskIdAsync(riskDetailId);
                var form = forms.FirstOrDefault(x => x.FormId == formId);
                return Ok(form);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<RiskFormDTO> resource)
        {
            try
            {
                return Ok(await _formsService.UpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Selected")]
        public async Task<IActionResult> PostSelected([FromBody] List<RiskFormDTO> resource)
        {
            return Ok(await _formsService.UpdateSelectedAsync(resource));
        }

        [HttpPost("Defaults/{id}")]
        public async Task<IActionResult> Post([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _formsService.InsertDefaultsAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("DefaultsNew/{id}")]
        public async Task<IActionResult> InsertDefaultsNew([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _formsService.InsertDefaultsNewAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("HasDriverExclusionForms/{id}")]
        [Produces(typeof(bool))]
        public async Task<IActionResult> GetDriverExclusionForms([FromRoute] Guid id)
        {
            try
            {
                var res = await _formsService.HasInitialDriverExclusionFormAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("User")]
        public async Task<IActionResult> Post()
        {
            try
            {
                if (Request.Form.Files.Count == 0)
                {
                    return BadRequest("No file found from request.");
                }

                Request.Form.TryGetValue("riskForm", out var riskFormValue);
                var riskFormDto = JsonConvert.DeserializeObject<RiskFormDTO>(riskFormValue);
                riskFormDto.FormId = Guid.NewGuid().ToString();
                foreach (var file in Request.Form.Files)
                {
                    await _storageService.Upload(file, $"{riskFormDto.FormId}/{file.FileName}");
                }
                return Ok(await _formsService.InsertUserFormAsync(riskFormDto));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("updateForm")]
        public async Task<IActionResult> Post([FromBody] RiskFormDTO riskForm)
        {
            try
            {
                return Ok(await _formsService.SingleFormUpdateAsync(riskForm));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotes([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _formsService.RemoveFormAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("UpdateFormEffectiveDate/{riskDetailId}")]
        public async Task<IActionResult> UpdateFormEffectiveDate([FromRoute] Guid riskDetailId)
        {
            try
            {
                var riskDetail = await _riskDetail.GetOneIncludeByIdAsync(riskDetailId);
                await _formsService.SaveFormsContentAsync(riskDetail, isEffectiveDateChanged: true);

                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}