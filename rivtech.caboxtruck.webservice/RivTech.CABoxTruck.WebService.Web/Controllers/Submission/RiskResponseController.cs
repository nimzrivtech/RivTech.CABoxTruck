using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskResponseController : ControllerBase
    {
        private readonly IRiskResponseService _riskResponseService;
        public RiskResponseController(
            IRiskResponseService RiskResponseService
        )
        {
            _riskResponseService = RiskResponseService;
        }

        [HttpGet("{riskDetailId}")]
        [Produces(typeof(List<RiskResponseDTO>))]
        public async Task<IActionResult> GetRiskResponse([FromRoute] Guid riskDetailId)
        {
            try
            {
                var res = await _riskResponseService.GetRiskResponse(riskDetailId);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{sectionId}/{riskDetailId}")]
        [Produces(typeof(List<RiskResponseDTO>))]
        public async Task<IActionResult> SaveRiskResponse([FromBody] List<RiskResponseDTO> riskResponses, [FromRoute] Int16 sectionId, [FromRoute] Guid riskDetailId)
        {
            try
            {
                //var riskResponses = new List<RiskResponseDTO>()
                //{
                //    new RiskResponseDTO()
                //    {
                //        QuestionId = 1,
                //        ResponseValue = "Hey NEWEST",
                //        RiskResponseTypeId = 1
                //    }
                //};
                var res = await _riskResponseService.SaveRiskResponse(riskResponses, sectionId, riskDetailId);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}