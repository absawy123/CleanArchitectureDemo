using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Dtos.Auth;
using WebApp.Application.Interfaces;

namespace WebAppDemo.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(string email , string password )
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var token =await _authService.LoginAsync(email, password);
            return Ok(token);

        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var isSuccessfull = await _authService.RegisterAsync(registerDto);
            if(isSuccessfull)
            return Ok();

            return BadRequest("Can not register.");
        }


    }
}
