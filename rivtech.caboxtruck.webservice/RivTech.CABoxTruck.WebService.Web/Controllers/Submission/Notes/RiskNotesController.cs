using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Notes;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskNotesController : ControllerBase
    {
        private readonly IRiskNotesService _riskNotesService;
        private readonly IRiskService _riskService;
        public RiskNotesController(IRiskNotesService riskNotesService, IRiskService riskService)
        {
            _riskNotesService = riskNotesService;
            _riskService = riskService;
        }

        [HttpPost]
        [Produces(typeof(RiskNotesDTO))]
        public async Task<IActionResult> SaveNotes([FromBody] RiskNotesDTO resource)
        {
            var entity = await _riskService.IsExistAsync(resource.RiskDetailId);
            if (!entity) return BadRequest();
            return Ok(await _riskNotesService.InsertorUpdateNotesAsync(resource));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotes([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _riskNotesService.RemoveAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

    }
}