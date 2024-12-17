using Microsoft.AspNetCore.Mvc;
using Homework_track_API.Services.AuthService;
using Homework_track_API.DTOs;

namespace Homework_track_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    
    public class AuthController(IAuthService authService): ControllerBase
    {
        private readonly IAuthService _authService = authService;
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Register(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Login(request);
            if (!result.IsSuccess)
            {
                return Unauthorized(result.ErrorMessage);
            }

            return Ok(result);
        }
    }
}

