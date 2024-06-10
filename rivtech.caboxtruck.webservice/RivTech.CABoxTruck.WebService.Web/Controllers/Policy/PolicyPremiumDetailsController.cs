using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Policy;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers.Policy
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PolicyPremiumDetailsController : ControllerBase
    {
        private readonly IPremiumService _premiumService;

        public PolicyPremiumDetailsController(
            IPremiumService premiumService
        )
        {
            _premiumService = premiumService;
        }

        [HttpGet("details/{riskId}")]
        [Produces(typeof(PolicyPremiumDetailsDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid riskId)
        {
            var data = await _premiumService.GetPolicyPremiumDetailsAsync(riskId);
            return Ok(data);
        }
    }
}
