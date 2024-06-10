using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.Submission.RiskSpecifics.MaintenanceSafety;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;

namespace RivTech.CABoxTruck.WebService.Controllers.Submission.RiskSpecifics
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RiskSpecificSafetyDeviceController : ControllerBase
    {
        private readonly IRiskSpecificSafetyDeviceService _riskSpecificSafetyDeviceService;
        public RiskSpecificSafetyDeviceController(
            IRiskSpecificSafetyDeviceService riskSpecificSafetyDeviceService
        )
        {
            _riskSpecificSafetyDeviceService = riskSpecificSafetyDeviceService;
        }

        [HttpGet("{riskDetailId}")]
        [Produces(typeof(List<RiskSpecificSafetyDeviceDTO>))]
        public async Task<IActionResult> GetRiskSpecificSafetyDevice([FromRoute] Guid riskDetailId)
        {
            try
            {
                var res = await _riskSpecificSafetyDeviceService.GetRiskSpecificSafetyDevice(riskDetailId);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("{riskDetailId}")]
        [Produces(typeof(List<RiskSpecificSafetyDeviceDTO>))]
        public async Task<IActionResult> SaveRiskSpecificSafetyDevice([FromBody] List<RiskSpecificSafetyDeviceDTO> safetyDevices, [FromRoute] Guid riskDetailId)
        {
            try
            {
                //safetyDevices.Add(new RiskSpecificSafetyDeviceDTO()
                //{
                //    RiskId = Guid.NewGuid(),
                //    SafetyDeviceCategoryId = 19,
                //    IsInPlace = true,
                //    UnitDescription = "Test Geographic Driving History Data",
                //    YearsInPlace = 2,
                //    PercentageOfFleet = 35
                //});
                var res = await _riskSpecificSafetyDeviceService.SaveRiskSpecificSafetyDevice(safetyDevices, riskDetailId);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}