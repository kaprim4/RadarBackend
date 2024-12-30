
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Auth;

namespace RadarBackend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("autoConnect")]
        public async Task<IActionResult> Login()
        {
            var output = await _userService.Login(new LoginDTO() { UserName = "othmanox", Password = "Admin@123"});
            if (!output.IsAuthentificated) return Unauthorized(output);

            return Ok(output);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var output = await _userService.Login(request);
            if (!output.IsAuthentificated) return Unauthorized(output);

            return Ok(output);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            var output = await _userService.Register(request);
            return Ok(output);
        }
    }
}