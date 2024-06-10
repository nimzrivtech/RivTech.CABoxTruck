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
    public class ErrorLogController : ControllerBase
    {
        private readonly ILog4NetService _service;

        public ErrorLogController(ILog4NetService service)
        {
            _service = service;
        }

        [HttpPost]
        [Produces(typeof(ErrorLogDTO))]
        public async Task<IActionResult> Post([FromBody] ErrorLogDTO resource)
        {
            try
            {
                ErrorLogDTO errLog = new ErrorLogDTO
                {
                    UserId = Data.Common.Services.GetCurrentUser(),
                    ErrorMessage = resource.ErrorMessage,
                    ErrorCode = resource.ErrorCode,
                    JsonMessage = resource.JsonMessage,
                    Action = resource.Action,
                    Method = resource.Method,
                    CreatedDate = DateTime.Now
                };
                await _service.ErrorAsync(errLog);
                return Ok();
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
