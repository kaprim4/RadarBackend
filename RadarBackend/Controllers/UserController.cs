
using AutoMapper.Execution;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Auth;
using System.Xml.Linq;
using System;

namespace RadarBackend.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("List")]
        public async Task<IActionResult> GetAll(PagableDTO<UserDTO> pagable) {
            var resp = await _userService.List(pagable);
            return Ok(resp);
        }
    }
}

