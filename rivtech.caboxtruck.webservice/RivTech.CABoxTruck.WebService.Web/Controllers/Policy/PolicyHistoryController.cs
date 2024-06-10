using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PolicyHistoryController : ControllerBase
    {
        private readonly IPolicyHistoryService _policyHistoryService;

        public PolicyHistoryController(IPolicyHistoryService policyHistoryService)
        {
            _policyHistoryService = policyHistoryService;
        }

        [HttpGet("{id}")]
        [Produces(typeof(List<PolicyHistoryDTO>))]
        public async Task<IActionResult> GetPolicyHistory([FromRoute] Guid id)
        {
            try
            {
                var resources = await _policyHistoryService.GetEndorsementHistoryByRiskId(id);
                return Ok(resources);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}

