using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers.Risk
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskController : ControllerBase
    {
        private readonly IRiskService _riskService;

        public RiskController(IRiskService riskService)
        {
            _riskService = riskService;
        }

        [HttpGet("GetAll")]
        [Produces(typeof(List<RiskDTO>))]
        public async Task<IActionResult> GetAll()
        {
            //GET ALL risk entity
            try
            {
                var res = await _riskService.GetAllAsync();
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetAllInclude")]
        [Produces(typeof(List<RiskDTO>))]
        public async Task<IActionResult> GetAllInclude()
        {
            //GET ALL risk entity
            try
            {
                var res = await _riskService.GetAllIncludeAsync();
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        //[HttpPost("summary/list")]
        //[Produces(typeof(List<RiskSummaryDto>))]
        //public async Task<IActionResult> GetRiskSummaryList([FromBody] SubmissionFilterDto filter)
        //{
        //    return Ok(await _riskService.GetFilteredSummaryListAsync(filter));
        //}

        [HttpGet("GetByRiskInclude/{id}")]
        [Produces(typeof(List<RiskDTO>))]
        public async Task<IActionResult> GetByRiskInclude([FromRoute] Guid id)
        {
            try
            {
                var res = await _riskService.GetByIdIncludeAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("Put/{id}")]
        [Produces(typeof(RiskDTO))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] RiskDTO resource)
        {
            try
            {
                try
                {
                    if (resource.Id != id)
                    {
                        return BadRequest();
                    }

                    return Ok(await _riskService.UpdateRiskAsync(resource));
                }
                catch (ModelNotFoundException)
                {
                    return NotFound();
                }
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("Status/{id}")]
        public async Task<IActionResult> GetStatus([FromRoute] Guid id)
        {
            try
            {
                var res = await _riskService.GetRiskAsync(id);
                return Ok(res.Status);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("Status/{id}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] RiskDTO resource)
        {
            try
            {
                if (resource.Id != id) { return BadRequest(); }
                return Ok(await _riskService.UpdateRiskStatusAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("UpdateEndorsementStatus/{id}")]
        public async Task<IActionResult> UpdateStatusToPendingEndorsement([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _riskService.UpdatePendingEndorsementAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Post")]
        [Produces(typeof(RiskDTO))]
        public async Task<IActionResult> Post([FromBody] RiskDTO resource)
        {
            try
            {
                //if (resource.Id != null) return BadRequest();
                return Ok(await _riskService.InsertRiskAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("SearchRiskByPolicyNumber/{policyNumber}/{zipCode}")]
        [Produces(typeof(List<PaymentPortalSearchResponseDTO>))]
        [AllowAnonymous]
        public async Task<IActionResult> SearchRiskByPolicyNumberAndZipCode([FromRoute] string policyNumber, string zipCode)
        {
            try
            {
                var res = await _riskService.SearchRiskByPolicyAndZipCodeAsync(policyNumber, zipCode);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetServerDateTime")]
        [AllowAnonymous]
        public IActionResult GetServerDateAndTime()
        {
            return Ok(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"));
        }

        [HttpGet("GetLatestRiskDetailVersion/{riskId}")]
        public async Task<Guid> GetLatestVersion(Guid riskId)
        {
            var riskDetailId = await _riskService.GetLatestRiskDetailVersionAsync(riskId);
            return riskDetailId;
        }
    }
}
