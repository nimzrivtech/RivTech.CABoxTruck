using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.GeneralLiabilityCargo;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission.RiskSpecifics
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskSpecificGeneralLiabilityCargoController : ControllerBase
    {
        private readonly IRiskSpecificGeneralLiabilityCargoService _riskSpecificGeneralLiabilityCargoService;
        public RiskSpecificGeneralLiabilityCargoController(
            IRiskSpecificGeneralLiabilityCargoService riskSpecificGeneralLiabilityCargoService
        )
        {
            _riskSpecificGeneralLiabilityCargoService = riskSpecificGeneralLiabilityCargoService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveRiskSpecificGeneralLiabilityCargo([FromBody] RiskSpecificGeneralLiabilityCargoDTO generalLiabilityCargo)
        {
            try
            {                
                var res = await _riskSpecificGeneralLiabilityCargoService.Save(generalLiabilityCargo);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, [FromBody] List<int> sectionIds)
        {
            try
            {
                var res = await _riskSpecificGeneralLiabilityCargoService.Delete(id, sectionIds);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}