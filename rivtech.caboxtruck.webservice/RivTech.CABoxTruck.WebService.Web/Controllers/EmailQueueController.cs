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
    [ApiController]
    [Authorize]
    public class EmailQueueController : ControllerBase
    {
        private readonly IEmailQueueService _emailQueueService;

        public EmailQueueController(IEmailQueueService emailQueueService)
        {
            _emailQueueService = emailQueueService;
        }

        [HttpPost("AddEmailQueue")]
        [Produces(typeof(EmailQueueDTO))]
        public async Task<IActionResult> AddEmailQueue([FromBody] EmailQueueDTO emailQueueDTO)
        {
            try
            {
                return Ok(await _emailQueueService.InsertEmailQueueAsync(emailQueueDTO));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("UpdateEmailQueue")]
        [Produces(typeof(EmailQueueDTO))]
        public async Task<IActionResult> UpdateEmailQueue([FromBody] EmailQueueDTO emailQueueDTO)
        {
            try
            {
                await _emailQueueService.UpdateEmailQueueAsync(emailQueueDTO);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("DeleteEmailQueue")]
        [Produces(typeof(EmailQueueDTO))]
        public async Task<IActionResult> DeleteEmailQueue([FromBody] EmailQueueDTO emailQueueDTO)
        {
            try
            {
                await _emailQueueService.DeleteEmailQueueAsync(emailQueueDTO);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetAllEmailQueued")]
        [Produces(typeof(List<EmailQueueDTO>))]
        public async Task<IActionResult> GetAdditionalTerms()
        {
            var resources = await _emailQueueService.GetAllAsync();
            return new ObjectResult(resources);
        }

        [HttpGet("GetEmailQueue/{id}")]
        [Produces(typeof(EmailQueueDTO))]
        public async Task<IActionResult> GetAdditionalTerm([FromRoute] Guid id)
        {
            try
            {
                var res = await _emailQueueService.GetEmailQueueAsync(id);
                return Ok(res);
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}