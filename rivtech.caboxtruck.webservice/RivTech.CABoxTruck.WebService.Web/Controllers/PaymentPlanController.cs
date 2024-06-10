using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Exceptions;
using RivTech.CABoxTruck.WebService.Service.IServices;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentPlansController : ControllerBase
    {
        private readonly IPaymentPlanService _paymentPlanService;
        public PaymentPlansController(IPaymentPlanService paymentPlanService)
        {
            _paymentPlanService = paymentPlanService;
        }

        [HttpGet]
        [Produces(typeof(List<PaymentPlanDTO>))]
        public async Task<IActionResult> GetPaymentPlans()
        {
            var resources = await _paymentPlanService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("{id}")]
        [Produces(typeof(PaymentPlanDTO))]
        public async Task<IActionResult> GetPaymentPlan([FromRoute] byte id)
        {
            try
            {
                var res = await _paymentPlanService.GetPaymentPlanAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(typeof(PaymentPlanDTO))]
        public async Task<IActionResult> PostPaymentPlan([FromBody] PaymentPlanDTO resource)
        {
            return Ok(await _paymentPlanService.InsertPaymentPlanAsync(resource));
        }

        [HttpPut("{id}")]
        [Produces(typeof(PaymentPlanDTO))]
        public async Task<IActionResult> PutPaymentPlan([FromRoute] byte id, [FromBody] PaymentPlanDTO resource)
        {
            try
            {
                if (resource.Id != id) return BadRequest();
                return Ok(await _paymentPlanService.UpdatePaymentPlanAsync(resource));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Produces(typeof(PaymentPlanDTO))]
        public async Task<IActionResult> DeletePaymentPlan([FromRoute] byte id)
        {
            try
            {
                await _paymentPlanService.RemovePaymentPlanAsync(id);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

    }
}