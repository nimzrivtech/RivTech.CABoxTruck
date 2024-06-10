using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PolicyContactController : ControllerBase
    {
        private readonly IRiskPolicyContactService _riskPolicyContactService;
        private readonly IPolicyContactsService _policyContactService;

        public PolicyContactController(IRiskPolicyContactService riskPolicyContactService, IPolicyContactsService policyContactService)
        {
            _riskPolicyContactService = riskPolicyContactService;
            _policyContactService = policyContactService;
        }

        [HttpGet("{id}")]
        [Produces(typeof(RiskPolicyContactDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _riskPolicyContactService.GetAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetByRiskDetailId/{riskDetailId}")]
        [Produces(typeof(List<RiskPolicyContactDTO>))]
        public async Task<IActionResult> GetByRiskDetailId([FromRoute] Guid riskDetailId)
        {
            return new ObjectResult(await _riskPolicyContactService.GetByRiskDetailIdAsync(riskDetailId));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RiskPolicyContactDTO resource)
        {
            return Ok(await _riskPolicyContactService.InsertAsync(resource));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] RiskPolicyContactDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                return Ok(await _riskPolicyContactService.UpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, bool fromEndorsement = false)
        {
            try
            {
                return Ok(await _riskPolicyContactService.RemoveAsync(id, fromEndorsement));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetAllAssignedToPolicyContacts")]
        [Produces(typeof(List<PolicyContactsDTO>))]
        public async Task<IActionResult> GetAllAssignedToPolicyContacts()
        {
            try
            {
                return Ok(await _policyContactService.GetAllAssignedToPolicyContacts());
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}