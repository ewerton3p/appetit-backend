using Appetit.Application.DTOs.User;
using Appetit.Application.Interfaces;
using Appetit.Domain.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Appetit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register(UserCreateDTO newUser)
        {
            return Created(nameof(Register), await _userService.Register(newUser));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseData<LoginResponseDTO>>> Login(LoginDTO loginDTO)
        {
            return Ok(await _userService.Login(loginDTO));
        }
    }
}
