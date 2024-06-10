using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Vehicles;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly ICoverageHistoryService _coverageHistoryService;
        private readonly IConfiguration _config;
        private readonly ILvDatasetService _lvDatasetService;

        private const string TEMPLATE_FILENAME = "Vehicles-Upload-Template.xlsx";
        private const string TEMPLATE_FILENAMEV7 = "Vehicles-Upload-Template-V7.xlsx";

        public VehicleController(IVehicleService vehicleService, ICoverageHistoryService coverageHistoryService, IConfiguration config, ILvDatasetService lvDatasetService)
        {
            _vehicleService = vehicleService;
            _coverageHistoryService = coverageHistoryService;
            _config = config;
            _lvDatasetService = lvDatasetService;
        }

        [HttpGet("GetAll")]
        [Produces(typeof(List<VehicleDTO>))]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _vehicleService.GetAllAsync();
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetAllInclude")]
        [Produces(typeof(List<VehicleDTO>))]
        public async Task<IActionResult> GetAllInclude()
        {
            try
            {
                var res = await _vehicleService.GetAllIncludeAsync();
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetByRisk/{id}")]
        [Produces(typeof(List<VehicleDTO>))]
        public async Task<IActionResult> GetByRisk([FromRoute] Guid id)
        {
            try
            {
                var res = await _vehicleService.GetByRiskDetailIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetByRiskInclude/{id}")]
        [Produces(typeof(List<VehicleDTO>))]
        public async Task<IActionResult> GetByRiskInclude([FromRoute] Guid id,
            [FromQuery] bool? fromPolicy = false)
        {
            try
            {
                var res = await _vehicleService.GetByRiskDetailIdIncludeAsync(id, fromPolicy, true);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
        [HttpGet("GetByRiskIncludePolicy/{id}")]
        [Produces(typeof(List<VehicleDTO>))]
        public async Task<IActionResult> GetByRiskIncludePolicy([FromRoute] Guid id,
        [FromQuery] bool? fromPolicy = false)
        {
            try
            {
                var res = await _vehicleService.GetByRiskDetailIdIncludePolicyAsync(id, fromPolicy, true);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("Get/{id}")]
        [Produces(typeof(VehicleDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var res = await _vehicleService.GetByIdAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetInclude/{id}")]
        [Produces(typeof(VehicleDTO))]
        public async Task<IActionResult> GetInclude([FromRoute] Guid id)
        {
            try
            {
                var res = await _vehicleService.GetByIdIncludeAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetPreviousVehicles/{id}")]
        [Produces(typeof(VehicleDTO[]))]
        public async Task<IActionResult> GetPreviousVehicles([FromRoute] Guid id)
        {
            try
            {
                var res = await _vehicleService.GetPreviousVehicles(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("HasPreviousVehicleVersions/{vehicleId}")]
        public async Task<IActionResult> HasPreviousVehicleVersions([FromRoute] Guid vehicleId)
        {
            var previousVehicles = await _vehicleService.GetPreviousVehicles(vehicleId);
            return Ok(previousVehicles?.Any() ?? false);
        }

        [HttpPut("PutListOptions")]
        [Produces(typeof(VehicleDTO))]
        public async Task<IActionResult> PutListOptions([FromBody] List<VehicleDTO> resource)
        {
            try
            {
                try
                {
                    return Ok(await _vehicleService.UpdateListOptionAsync(resource));
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

        [HttpPut("{id}")]
        [Produces(typeof(VehicleDTO))]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] VehicleDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                if (await _vehicleService.IsOldVersion(id))
                    throw new InvalidOperationException($"Cannot update an old version of vehicle.");
                return Ok(await _vehicleService.UpdateAsync(resource));
            }
            catch (ArgumentException ex)
            {
                return Ok(new { Message = ex.Message });
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("Addresses")]
        [Produces(typeof(VehicleDTO))]
        public async Task<IActionResult> PutAddresses([FromBody] List<VehicleDTO> resource)
        {
            try
            {
                return Ok(await _vehicleService.UpdateAddressesAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("OtherCoverages")]
        [Produces(typeof(VehicleDTO))]
        public async Task<IActionResult> PutOtherCoverages([FromBody] JObject resource)
        {
            try
            {
                List<VehicleDTO> vehicleList = resource.SelectToken("vehicles")?.ToObject<List<VehicleDTO>>();
                RiskCoverageDTO oldLimitsData = resource.SelectToken("oldLimitsData")?.ToObject<RiskCoverageDTO>();
                return Ok(await _vehicleService.UpdateOtherCoveragesAsync(vehicleList, oldLimitsData));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(VehicleDTO))]
        public async Task<IActionResult> Post([FromBody] VehicleDTO resource)
        {
            try
            {
                if (resource.Id != null) return BadRequest();
                return Ok(await _vehicleService.InsertAsync(resource));
            } 
            catch (ArgumentException ex)
            {
                return Ok(new { Message = ex.Message });
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("SaveVehicleHeader")]
        [Produces(typeof(VehicleDTO))]
        public async Task<IActionResult> SaveVehicleHeader([FromBody] VehicleHeaderDTO resource)
        {
            try
            {
                await _vehicleService.UpdateVehicleHeader(resource);
                return Ok(new VehicleDTO());
            }
            catch(ArgumentException ex)
            {
                return Ok(new { Message = ex.Message });
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpDelete("{id}")]
        [Produces(typeof(VehicleDTO))]
        public async Task<IActionResult> Delete([FromRoute] Guid id, bool? fromEndorsement = null)
        {
            try
            {
                if (id == null || id == Guid.Empty) return BadRequest();
                if (await _vehicleService.IsOldVersion(id))
                    throw new InvalidOperationException($"Cannot remove an old version({id}) of vehicle.");


                var riskDetailId = await _vehicleService.RemoveAsync(id, fromEndorsement);

                // var deletedRecord = await _vehicleService.GetByIdAsync(id);
                var res = await _vehicleService.GetByRiskDetailIdIncludeAsync(Guid.Parse(riskDetailId), fromEndorsement);
                if (res != null && res.Count > 0 && res.Count <= 10) await _coverageHistoryService.DeleteFieldsAsync(res[0].RiskDetailId.Value, true, false, false, false, false, false);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("DeleteAll/{id}")]
        [Produces(typeof(VehicleDTO))]
        public async Task<IActionResult> DeleteAll([FromRoute] Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty) return BadRequest();
                
                var riskDetailId = await _vehicleService.RemoveAllVinAsync(id);

                // var deletedRecord = await _vehicleService.GetByIdAsync(id);
                var res = await _vehicleService.GetByRiskDetailIdIncludeAsync(Guid.Parse(riskDetailId), false);
                if (res != null && res.Count > 0 && res.Count <= 10) await _coverageHistoryService.DeleteFieldsAsync(res[0].RiskDetailId.Value, true, false, false, false, false, false);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("Reinstate/{id}")]
        [Produces(typeof(VehicleDTO))]
        public async Task<IActionResult> Reinstate([FromRoute] Guid id)
        {
            if (id == Guid.Empty) 
                return BadRequest("Invalid id");
            var vehicle = await _vehicleService.ReinstateVehicleAsync(id);
            return Ok(vehicle);
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
                var vehicles = await _vehicleService.AddFromExcelFileAsync(postedFile.OpenReadStream());

                return Ok(vehicles);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("ExcelVehicles")]
        [Produces(typeof(VehicleDTO))]
        public async Task<IActionResult> Post([FromBody] List<VehicleDTO> resource)
        {
            try
            {
                return Ok(await _vehicleService.InsertRangeAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("template")]
        public async Task<ActionResult> Download()
        {
            var fileBytes = await System.IO.File.ReadAllBytesAsync($"{FilesFolder}\\{TEMPLATE_FILENAME}");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", TEMPLATE_FILENAME);
        }
        [HttpGet]
        [Route("templateV7")]
        public async Task<ActionResult> DownloadV7()
        {
            var fileBytes = await System.IO.File.ReadAllBytesAsync($"{FilesFolder}\\{TEMPLATE_FILENAMEV7}");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", TEMPLATE_FILENAMEV7);
        }

        //public string UploadFolder => _config.GetSection("Rater").GetValue<string>("ExperienceRatersUploadDirectory");
        public string FilesFolder => ".\\FileShares";

        [HttpGet("GetVehicleGVWDataset")]
        [Produces(typeof(LvDatasetDTO[]))]
        public async Task<IActionResult> GetVehicleGVWDataset([FromQuery] bool isCurrentlySupported)
        {
            try
            {
                var res = await _lvDatasetService.GetByTypeAndRaterVersionAsync(LvDatasetType.VehicleGVW, isCurrentlySupported);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpGet("GetVehicleDescriptionDataset")]
        [Produces(typeof(LvDatasetDTO[]))]
        public async Task<IActionResult> GetVehicleDescriptionDataset([FromQuery] bool isCurrentlySupported)
        {
            try
            {
                var res = await _lvDatasetService.GetByTypeAndRaterVersionAsync(LvDatasetType.VehicleDescription, isCurrentlySupported);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

    }
}