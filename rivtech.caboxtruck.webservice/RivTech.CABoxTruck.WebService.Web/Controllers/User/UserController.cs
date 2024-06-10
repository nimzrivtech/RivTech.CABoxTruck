using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RivTech.CABoxTruck.WebService.DTO.User;
using RivTech.CABoxTruck.WebService.Service.IServices.User;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInfoService _userInfoService;

        public UserController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        [HttpPost("login")]
        [Produces("application/json")]
        public async Task<ActionResult<string>> LoginAsync([FromBody] LoginDto model)
        {
            return Ok(await _userInfoService.LoginAsync(model));
        }

        [HttpPost("logout")]
        [Produces("application/json")]
        [Authorize]
        public async Task<ActionResult<string>> LogoutAsync([FromBody] LoginDto model)
        {
            return Ok(await _userInfoService.LogoutAsync(model));
        }

        [HttpPost("failed")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> FailedAsync([FromBody] LoginDto model)
        {
            return Ok(await _userInfoService.FailedLoginAsync(model));
        }

        [HttpPost("lock")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> LockAsync([FromBody] LoginDto model)
        {
            return Ok(await _userInfoService.LockAccountAsync(model));
        }

        [HttpPost("locked")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> LockedAsync([FromBody] LoginDto model)
        {
            return Ok(await _userInfoService.LockedAccountAsync(model));
        }
    }
}
