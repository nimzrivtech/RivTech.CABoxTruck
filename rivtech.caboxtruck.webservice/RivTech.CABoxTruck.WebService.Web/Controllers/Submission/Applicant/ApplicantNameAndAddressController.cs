using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ApplicantNameAndAddressController : ControllerBase
    {
        private readonly INameAndAddressService _nameAndAddressService;
        private readonly IRiskService _riskService;
        private readonly IBillingService _billingService;

        public ApplicantNameAndAddressController(INameAndAddressService nameAndAddressService, IRiskService riskService, IBillingService billingService)
        {
            _nameAndAddressService = nameAndAddressService;
            _riskService = riskService;
            _billingService = billingService;
        }

        [HttpGet("{id}")]
        [Produces(typeof(NameAndAddressDTO))]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var exist = await _riskService.IsExistAsync(id);
            if (!exist) return BadRequest();
            var res = await _nameAndAddressService.GetNameAndAddress(id);
            return Ok(res);
        }

        [HttpGet("GetAddress/{id}")]
        [Produces(typeof(AddressDTO))]
        public async Task<IActionResult> GetAddress([FromRoute] Guid id)
        {
            return Ok(await _nameAndAddressService.GetAddress(id));
        }

        [HttpPost]
        [Produces(typeof(NameAndAddressDTO))]
        public async Task<IActionResult> SaveRiskNameAndAddress([FromBody] NameAndAddressDTO resource)
        {
            try
            {
                return Ok(await _nameAndAddressService.InsertAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Produces(typeof(NameAndAddressDTO))]
        public async Task<IActionResult> UpdateNameAndAddress([FromBody] NameAndAddressDTO resource)
        {
            try
            {
                return Ok(await _nameAndAddressService.UpdateAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetFeeDetailsByState/{stateCode}")]
        public IActionResult GetFeeDetailsByState([FromRoute]string stateCode)
        {
            try
            {
                var res = _billingService.GetFeeDetailsByState(stateCode);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("UpdateNameAndAddressMidterm")]
        [Produces(typeof(NameAndAddressDTO))]
        public async Task<IActionResult> UpdateNameAndAddressMidterm([FromBody] NameAndAddressDTO resource)
        {
            try
            {
                return Ok(await _nameAndAddressService.UpdateMidtermAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAddress([FromRoute] Guid id)
        {
            try
            {
                await _nameAndAddressService.RemoveAddressAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("InsertEntityAddress")]
        public async Task<IActionResult> InsertEntityAddress([FromBody] EntityAddressDTO resource)
        {
            try
            {
                await _nameAndAddressService.InsertEntityAddressAsync(resource);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}