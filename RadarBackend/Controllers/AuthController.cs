using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RadarBackend.Models;
using RadarBackend.Services;

namespace RadarBackend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _userService.Authenticate(request.Email, request.Password);
            if (user == null) return Unauthorized();

            return Ok(new { Token = "JWT_TOKEN_PLACEHOLDER" });
        }
    }
}