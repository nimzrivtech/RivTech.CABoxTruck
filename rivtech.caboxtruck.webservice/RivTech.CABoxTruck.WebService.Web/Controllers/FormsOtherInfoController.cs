using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FormsOtherInfoController : ControllerBase
    {
        private readonly IFormsService _formsService;
        private readonly IQuoteOptionsService _quoteOptionsService;

        public FormsOtherInfoController(IFormsService formsService, IQuoteOptionsService quoteOptionsService)
        {
            _formsService = formsService;
            _quoteOptionsService = quoteOptionsService;
        }

        [HttpPost("ExclusionRadius")]
        public async Task<IActionResult> PostExclusionRadius(RiskFormOtherDTO resource)
        {
            return Ok(await _formsService.UpdateOtherAsync<ExclusionRadiusDTO>(resource));
        }

        [HttpPost("HiredPhysdam")]
        public async Task<IActionResult> PostHiredPhysdam(RiskFormOtherDTO resource)
        {
            await _quoteOptionsService.UpdateHiredPhysicalDamage(resource);
            return Ok(await _formsService.UpdateOtherAsync<HiredPhysdamDTO>(resource));
        }

        [HttpPost("DriverRequirement")]
        public async Task<IActionResult> PostDriverRequirement(RiskFormOtherDTO resource)
        {
            return Ok(await _formsService.UpdateOtherAsync<DriverRequirementDTO>(resource));
        }

        [HttpPost("TerritoryExclusion")]
        public async Task<IActionResult> PostTerritoryExclusion(RiskFormOtherDTO resource)
        {
            return Ok(await _formsService.UpdateOtherAsync<TerritoryExclusionDTO>(resource));
        }

        [HttpPost("TrailerInterchange")]
        public async Task<IActionResult> PostTrailerInterchange(RiskFormOtherDTO resource)
        {
            await _quoteOptionsService.UpdateTrailerInterchange(resource);
            return Ok(await _formsService.UpdateOtherAsync<TrailerInterchangeDTO>(resource));
        }

        [HttpPost("GeneralLiability")]
        public async Task<IActionResult> PostGeneralLiability(RiskFormOtherDTO resource)
        {
            return Ok(await _formsService.UpdateOtherAsync<GeneralLiabilityDTO>(resource));
        }

        [HttpPost("Mcs90")]
        public async Task<IActionResult> PostMcs90(RiskFormOtherDTO resource)
        {
            return Ok(await _formsService.UpdateOtherAsync<Mcs90DTO>(resource));
        }

        [HttpPost("DefaultStates/{id}")]
        public async Task<IActionResult> PostDefaultStates([FromRoute] Guid id)
        {
            return Ok(await _formsService.UpdateDefaultStatesAsync(id));
        }

        [HttpPost("LossPayee")]
        public async Task<IActionResult> PostLossPayee(RiskFormOtherDTO resource)
        {
            return Ok(await _formsService.UpdateOtherAsync<FormLossPayeeDTO>(resource));
        }

        [HttpPost("StatePAaddcombFPB")]
        public async Task<IActionResult> PostStatePAaddcombFPB(RiskFormOtherDTO resource)
        {
            return Ok(await _formsService.UpdateOtherAsync<FormPAaddcombFPBDTO>(resource));
        }

        [HttpPost("CoveredAutoDesignationSymbol")]
        public async Task<IActionResult> PostCoveredAutoDesignationSymbol(RiskFormOtherDTO resource)
        {
            return Ok(await _formsService.UpdateOtherAsync<CoveredAutoDesigSymbolDTO>(resource));
        }

        [HttpGet("SymbolTenResponses/{riskDetailId}")]
        public async Task<IActionResult> GetSymbolTenResponses([FromRoute] Guid riskDetailId)
        {
            return Ok(await _formsService.GetSymbolTenResponses(riskDetailId));
        }
    }
}