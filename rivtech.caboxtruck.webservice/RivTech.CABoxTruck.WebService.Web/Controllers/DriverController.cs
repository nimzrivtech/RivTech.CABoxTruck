using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize] // this not advisable but for now disabled to allow form upload file
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly ILogger<DriverController> _logger;
        private readonly IDriverService _service;
        private readonly ICoverageHistoryService _coverageHistoryService;

        private const string TEMPLATE_FILENAME = "Driver-Upload-Template.xlsx";

        public DriverController(
            ILogger<DriverController> logger,
            IDriverService service,
            ICoverageHistoryService coverageHistoryService
        )
        {
            _logger = logger;
            _service = service;
            _coverageHistoryService = coverageHistoryService;
        }

        [HttpGet("GetAll")]
        [Produces(typeof(List<DriverDTO>))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _service.GetAllAsync();
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetAllInclude")]
        [Produces(typeof(List<DriverDTO>))]
        public async Task<IActionResult> GetAllInclude()
        {
            try
            {
                var res = await _service.GetAllIncludeAsync();
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetByRisk/{id}")]
        [Produces(typeof(List<DriverDTO>))]
        public async Task<IActionResult> GetByRisk([FromRoute] Guid id)
        {
            try
            {
                var res = await _service.GetByRiskDetailIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetByRiskInclude/{id}")]
        [Produces(typeof(List<DriverDTO>))]
        public async Task<IActionResult> GetByRiskInclude([FromRoute] Guid id)
        {
            try
            {
                var res = await _service.GetByRiskDetailIdIncludeAllAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetByRiskIncludeExport/{id}")]
        [Produces(typeof(List<DriverDTO>))]
        public async Task<IActionResult> GetByRiskIncludeExport([FromRoute] Guid id, [FromQuery] bool? fromPolicy = false)
        {
            try
            {

                var res = new List<DriverDTO>();
                if (fromPolicy == true)
                {
                    res = await _service.GetByRiskDetailIdIncludeAllExportAsync(id);
                }
                else
                {
                    res = await _service.GetByRiskDetailIdIncludeAsync(id);
                }
                     
                
                return Ok(res.OrderBy(x=> x.CreatedDate));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        
        [HttpGet("Get/{id}")]
        [Produces(typeof(DriverDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _service.GetByIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetInclude/{id}")]
        [Produces(typeof(DriverDTO))]
        public async Task<IActionResult> GetInclude([FromRoute] Guid id)
        {
            try
            {
                var res = await _service.GetByIdIncludeAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/{fromEndorsement}")]
        [Authorize]
        [Produces(typeof(DriverDTO))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] DriverDTO resource, [FromRoute] bool? fromEndorsement = false)
        {
            try
            {
                try
                {
                    if (resource.Id != id) return BadRequest();
                    return Ok(await _service.UpdateAsync(resource, fromEndorsement));
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

        [HttpPut("PutListOptions")]
        [Produces(typeof(DriverDTO))]
        public async Task<IActionResult> PutListOptions([FromBody] List<DriverDTO> resource)
        {
            try
            {
                try
                {
                    return Ok(await _service.UpdateListOptionAsync(resource));
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

        [HttpPost("Post")]
        [Produces(typeof(DriverDTO))]
        public async Task<IActionResult> Post([FromBody] DriverDTO resource)
        {
            try
            {
                if (resource.Id != null) return BadRequest();
                return Ok(await _service.InsertAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("Delete/{id}")]
        [Produces(typeof(DriverDTO))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty) return BadRequest();
                var riskDetailId = await _service.RemoveAsync(id);
                var res = await _service.GetByRiskDetailIdIncludeAsync(Guid.Parse(riskDetailId));
                if (res != null && res.Count() <= 10 && res.Any()) await _coverageHistoryService.DeleteFieldsAsync(Guid.Parse(riskDetailId), false, true, false, false, false, false);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("DeleteAll/{id}")]
        [Produces(typeof(DriverDTO))]
        public async Task<IActionResult> DeleteAll([FromRoute] Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty) return BadRequest();
                var riskDetailId = await _service.RemoveAllDriverAsync(id);

                var res = await _service.GetByRiskDetailIdIncludeAsync(Guid.Parse(riskDetailId));
                if (res != null && res.Count() <= 10 && res.Any()) await _coverageHistoryService.DeleteFieldsAsync(Guid.Parse(riskDetailId), false, true, false, false, false, false);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        } 

        [HttpGet("GetPreviousDrivers/{id}")]
        [Produces(typeof(DriverDTO[]))]
        public async Task<IActionResult> GetPreviousDrivers([FromRoute] Guid id)
        {
            try
            {
                var res = await _service.GetPreviousDrivers(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("Reinstate/{id}")]
        [Produces(typeof(DriverDTO))]
        public async Task<IActionResult> Reinstate([FromRoute] Guid id)
        {
            if (id == null || id == Guid.Empty) 
                return BadRequest("Invalid id");
            var driver = await _service.ReinstateDriverAsync(id);
            return Ok(driver);
        }

        #region Driver Download Upload
        public string FilesFolder => ".\\FileShares";

        [HttpGet("template")]
        // [Route("template")]
        public async Task<ActionResult> Download()
        {
            var fileBytes = await System.IO.File.ReadAllBytesAsync($"{FilesFolder}\\{TEMPLATE_FILENAME}");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", TEMPLATE_FILENAME);
        }

        [HttpPost]
        [Route("AddFromExcel")]
        public async Task<IActionResult> AddFromExcel()
        {
            try
            {
                var postedFile = Request.Form.Files[0];
                if (postedFile.Length < 1)
                {
                    return BadRequest("No file found from request.");
                }
                var drivers = await _service.AddFromExcelFileAsync(postedFile.OpenReadStream());

                return Ok(drivers);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("ExcelDrivers")]
        [Produces(typeof(DriverDTO))]
        public async Task<IActionResult> Post([FromBody] List<DriverDTO> resource)
        {
            try
            {
                return Ok(await _service.InsertRangeAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        #endregion Driver Download Upload

        #region Driver Header


        [HttpGet("DriverHeader/{riskDetailId}")]
        public async Task<IActionResult> GetHeader(Guid riskDetailId)
        {
            return Ok(await _service.GetHeaderAsync(riskDetailId));
        }

        [HttpPost("DriverHeader/Update")]
        [Produces(typeof(DriverHeaderDTO))]
        public async Task<IActionResult> UpdateHeader([FromBody] DriverHeaderDTO resource)
        {
            try
            {
                return Ok(await _service.AddUpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
        #endregion Driver Header

        [HttpGet("GetDriversByLastNameAndLicenseNumber/{lastName}/{licenseNumber}")]
        [Produces(typeof(List<DriverDTO>))]
        public async Task<IActionResult> GetDriversByLastNameAndLicenseNumber([FromRoute] string lastName, string licenseNumber)
        {
            try
            {
                var res = await _service.GetDriversByLastNameAndLicenseNumberAsync(lastName, licenseNumber);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }



        [HttpPut("UpdateDriverExclusionEndorsementForm/{riskDetailId}")]
        public async Task<IActionResult> UpdateDriverExclusionEndorsementForm([FromRoute] Guid riskDetailId)
        {

            await _service.UpdateDriverExclusionEndorsementFormAsync(riskDetailId);
            return Ok();
        }
    }
}

//insert Driver
//values('046416a7-a424-497d-af55-1522a82ce333','046416a7-a424-497d-af55-1522a82ce378', '046416a7-a424-497d-af55-1522a82ce399','test', GETDATE(),24,2020, GETDATE(),2,2,1,null,null,null,null,1, GETDATE())

//insert[dbo].[DriverPeriod]
//values('046416a7-a424-497d-af55-1522a82ce222', '046416a7-a424-497d-af55-1522a82ce333', GETDATE(), GETDATE(), GETDATE(), GETDATE(), 123);

//insert[dbo].[DriverSummary]
//values('046416a7-a424-497d-af55-1522a82ce111', '046416a7-a424-497d-af55-1522a82ce333', 2020, 1, 2, 3, 4, 5, 6, 7, 8, 'abc123', 1, getdate())