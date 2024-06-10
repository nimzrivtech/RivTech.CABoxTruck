using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.Applicant;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.Applicant;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission.Applicant
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilingsInformationController : ControllerBase
    {
        private readonly IFilingsInformationService _filingsInformationService;
        private readonly IRiskFilingService _riskFilingService;
        private readonly IFormsService _formsService;

        public FilingsInformationController(IFilingsInformationService filingsInformationService,
            IRiskFilingService riskFilingService, IFormsService formsService)
        {
            _filingsInformationService = filingsInformationService;
            _riskFilingService = riskFilingService;
            _formsService = formsService;
        }


        [HttpGet("{id}")]
        [Produces(typeof(FilingsInformationDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var filingsInformation = await _filingsInformationService.GetAsync(id);
            if (!filingsInformation.IsFilingRequired.GetValueOrDefault(false)) return Ok(filingsInformation);

            var riskFiling = await _riskFilingService.GetByRiskDetailIdAsync(filingsInformation.RiskDetailId);
            filingsInformation.RequiredFilingIds = riskFiling.Select(rf => rf.FilingTypeId).ToList();
            return Ok(filingsInformation);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] FilingsInformationDTO resource)
        {
            try
            {
                if (resource.RiskDetailId != id) return BadRequest();

                await _riskFilingService.RemoveByRiskDetailIdAsync(resource.RiskDetailId);
                if (!resource.IsFilingRequired.GetValueOrDefault(false)) return Ok(await _filingsInformationService.UpdateAsync(resource));

                var riskFilings = resource.RequiredFilingIds.Select(filingId => new RiskFilingDTO
                    {
                        RiskDetailId = resource.RiskDetailId,
                        FilingTypeId = filingId,
                        IsActive = true,
                        CreatedBy = 0,
                        CreatedDate = DateTime.Now
                    })
                    .ToList();
                await _riskFilingService.InsertRangeAsync(riskFilings);

                await _formsService.RiskFormUpdateAsync(new RiskFormDTO
                {
                    RiskDetailId = resource.RiskDetailId,
                    FormId = "FormFMotorCarrierBIPD",
                    IsSelected = riskFilings.Any(rf => rf.FilingTypeId == 9 || rf.FilingTypeId == 11)
                });

                return Ok(await _filingsInformationService.UpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}