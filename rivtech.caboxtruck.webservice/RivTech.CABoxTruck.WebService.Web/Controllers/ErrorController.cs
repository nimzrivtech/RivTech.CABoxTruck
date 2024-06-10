using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.Errors;

namespace RivTech.CABoxTruck.WebService.Controllers
{

    [Route("errors/{statusCode}")]
    [ApiController]
    [Authorize]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Error(int statusCode)
        {
            return new ObjectResult(new ErrorResponse(statusCode));
        }
    }
}
