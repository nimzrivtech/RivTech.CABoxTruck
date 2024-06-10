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
    public class EmailQueueArchiveController : ControllerBase
    {
        private readonly IEmailQueueArchiveService _emailQueueArchiveService;

        public EmailQueueArchiveController(IEmailQueueArchiveService emailQueueArchiveService)
        {
            _emailQueueArchiveService = emailQueueArchiveService;
        }

        [HttpPost("AddEmailQueueArchive")]
        [Produces(typeof(EmailQueueArchiveDTO))]
        public async Task<IActionResult> AddEmailQueueArchive([FromBody] EmailQueueArchiveDTO emailQueueArchiveDTO)
        {
            try
            {
                await _emailQueueArchiveService.InsertEmailQueueArchiveAsync(emailQueueArchiveDTO);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("UpdateEmailQueueArchive")]
        [Produces(typeof(EmailQueueArchiveDTO))]
        public async Task<IActionResult> UpdateEmailQueueArchive([FromBody] EmailQueueArchiveDTO emailQueueArchiveDTO)
        {
            try
            {
                await _emailQueueArchiveService.UpdateEmailQueueArchiveAsync(emailQueueArchiveDTO);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("DeleteEmailQueueArchive")]
        [Produces(typeof(EmailQueueArchiveDTO))]
        public async Task<IActionResult> DeleteEmailQueueArchive([FromBody] EmailQueueArchiveDTO emailQueueArchiveDTO)
        {
            try
            {
                await _emailQueueArchiveService.DeleteEmailQueueArchiveAsync(emailQueueArchiveDTO);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}