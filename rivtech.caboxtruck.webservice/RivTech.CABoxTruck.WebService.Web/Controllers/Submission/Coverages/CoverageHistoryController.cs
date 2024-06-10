using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CoverageHistoryController : ControllerBase
    {
        private readonly ICoverageHistoryService _coverageHistoryService;

        public CoverageHistoryController(ICoverageHistoryService coverageHistoryService)
        {
            _coverageHistoryService = coverageHistoryService;
        }

        [HttpGet("{id}")]
        [Produces(typeof(RiskHistoryListDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _coverageHistoryService.GetByIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(RiskHistoryListDTO))]
        public async Task<IActionResult> SaveRiskCoverage([FromBody] RiskHistoryListDTO resource)
        {
            try
            {
                var data = await _coverageHistoryService.IsExistAsync(resource.RiskDetailId);
                if(!data)
                {
                    return Ok(await _coverageHistoryService.InsertAsync(resource));
                }
                return Ok(await _coverageHistoryService.UpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
            
        }
    }
}