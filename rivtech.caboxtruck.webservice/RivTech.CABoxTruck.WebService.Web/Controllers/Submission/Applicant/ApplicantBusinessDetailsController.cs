using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ApplicantBusinessDetailsController : ControllerBase
    {
        private readonly IBusinessDetailsService _businessDetailsService;
        private readonly IEntitySubsidiaryService _entitySubsidiaryService;
        private readonly IRiskService _riskService;
        private readonly ILvDatasetService _lvDatasetService;

        public ApplicantBusinessDetailsController(IBusinessDetailsService businessDetailsService, IEntitySubsidiaryService entitySubsidiaryService, IRiskService riskService,ILvDatasetService lvDatasetService)
        {
            _businessDetailsService = businessDetailsService;
            _riskService = riskService;
            _entitySubsidiaryService = entitySubsidiaryService;
            _lvDatasetService = lvDatasetService;
        }

        [HttpGet("{id}")]
        [Produces(typeof(BusinessDetailsDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var exist = await _riskService.IsExistAsync(id);
            if (!exist) return BadRequest();
            var res = await _businessDetailsService.GetBusinessDetails(id);
            return Ok(res);
        }

        [HttpPost]
        [Produces(typeof(BusinessDetailsDTO))]
        public async Task<IActionResult> Post([FromBody] BusinessDetailsDTO resource)
        {
            var exist = await _riskService.IsExistAsync(resource.RiskDetailId);
            if (!exist) return BadRequest();
            return Ok(await _businessDetailsService.InsertUpdateBusinessDetailsAsync(resource));
        }

        [HttpPost("SaveSubsidiary")]
        [Produces(typeof(EntitySubsidiaryDTO))]
        public async Task<IActionResult> SaveSubsidiary([FromBody] SaveEntitySubsidiaryDTO resource)
        {
            var entity = await _riskService.IsExistAsync(resource.RiskDetailId.Value);
            if (!entity) return BadRequest();
            return Ok(await _entitySubsidiaryService.InsertorUpdateSubsidiaryAsync(resource));
        }

        [HttpDelete("DeleteSubsidiaryAll/{id}")]
        public async Task<IActionResult> DeleteSubsidiaryAll([FromRoute] Guid id)
        {
            var entity = await _riskService.IsExistAsync(id);
            if (!entity) return BadRequest();
            return Ok(await _entitySubsidiaryService.RemoveSubsidiaryAllAsync(id));
        }

        [HttpDelete("DeleteSubsidiary/{id}")]
        public async Task<IActionResult> DeleteSubsidiary([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _entitySubsidiaryService.RemoveAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpPost("SaveCompany")]
        [Produces(typeof(EntitySubsidiaryDTO))]
        public async Task<IActionResult> SaveCompany([FromBody] SaveEntitySubsidiaryDTO resource)
        {
            var entity = await _riskService.IsExistAsync(resource.RiskDetailId.Value);
            if (!entity) return BadRequest();
            return Ok(await _entitySubsidiaryService.InsertorUpdateCompanyAsync(resource));
        }

        [HttpDelete("DeleteCompanyAll/{id}")]
        public async Task<IActionResult> DeleteCompanyAll([FromRoute] Guid id)
        {
            var entity = await _riskService.IsExistAsync(id);
            if (!entity) return BadRequest();
            return Ok(await _entitySubsidiaryService.RemoveCompanyAllAsync(id));
        }

        [HttpDelete("DeleteCompany/{id}")]
        public async Task<IActionResult> DeleteCompany([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _entitySubsidiaryService.RemoveAsync(id));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetMainGVWDataset")]
        [Produces(typeof(LvDatasetDTO[]))]
        public async Task<IActionResult> GetMainGVWDataset([FromQuery] bool isCurrentlySupported)
        {
            try
            {
                var res = await _lvDatasetService.GetByTypeAndRaterVersionAsync(LvDatasetType.MainGVW, isCurrentlySupported);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetSecondaryBusinessGroupDataset")]
        [Produces(typeof(LvDatasetDTO[]))]
        public async Task<IActionResult> GetSecondaryBusinessGroupDataset([FromQuery] bool isCurrentlySupported)
        {
            try
            {
                var res = await _lvDatasetService.GetByTypeAndRaterVersionAsync(LvDatasetType.SecondaryBusinessGroup, isCurrentlySupported);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}