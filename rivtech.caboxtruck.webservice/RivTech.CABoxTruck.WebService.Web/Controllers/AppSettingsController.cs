using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.User;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.User;
using System;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsController : ControllerBase
    {
        public AppSettingsController()
        {
        }

        [HttpGet]
        [Produces("application/json")]
        [Authorize]
        public ActionResult<Object> GetAppSettingsAsync()
        {
            return Ok(
                new
                {
                    ActivateInactivityTimerSecs = Environment.GetEnvironmentVariable("ActivateInactivityTimerSecs") ?? "5",
                    TriggerInactivityTimerSecs = Environment.GetEnvironmentVariable("TriggerInactivityTimerSecs") ?? "10"
                }
            );
        }
    }
}
