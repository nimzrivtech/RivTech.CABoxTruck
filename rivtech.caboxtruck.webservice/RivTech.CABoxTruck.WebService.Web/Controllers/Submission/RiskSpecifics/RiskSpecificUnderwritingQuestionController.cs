using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission.RiskSpecifics
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskSpecificUnderwritingQuestionController : ControllerBase
    {
        private readonly IRiskSpecificUnderwritingQuestionService _riskSpecificUnderwritingQuestionService;
        public RiskSpecificUnderwritingQuestionController(IRiskSpecificUnderwritingQuestionService riskSpecificUnderwritingQuestionService)
        {
            _riskSpecificUnderwritingQuestionService = riskSpecificUnderwritingQuestionService;
        }

        [HttpGet]
        [Produces(typeof(List<RiskSpecificUnderwritingQuestionDto>))]
        public async Task<IActionResult> GetAll()
        {
            var resources = await _riskSpecificUnderwritingQuestionService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(RiskSpecificUnderwritingQuestionDto))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _riskSpecificUnderwritingQuestionService.GetUnderwritingQuestionAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(RiskSpecificUnderwritingQuestionDto))]
        public async Task<IActionResult> Post([FromBody] RiskSpecificUnderwritingQuestionDto resource)
        {
            return Ok(await _riskSpecificUnderwritingQuestionService.InsertUnderwritingQuestionAsync(resource));
        }

        [HttpPut("risk/{riskDetailId}")]
        [Produces(typeof(RiskSpecificUnderwritingQuestionDto))]
        public async Task<IActionResult> Put([FromRoute] Guid riskDetailId, [FromBody] RiskSpecificUnderwritingQuestionDto resource)
        {
            return Ok(await _riskSpecificUnderwritingQuestionService.UpdateUnderwritingQuestionAsync(riskDetailId, resource));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                return Ok(_riskSpecificUnderwritingQuestionService.RemoveUnderwritingQuestionAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
