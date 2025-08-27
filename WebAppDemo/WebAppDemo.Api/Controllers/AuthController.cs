using Azure.Core;
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
            var tokens =await _authService.LoginAsync(email, password);
            if (tokens == null)
                return BadRequest("Invalid email or password");

            return Ok(tokens);

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

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid) 
                return BadRequest();
            var result = await _authService.ChangePasswordAsync(dto.Email ,dto.CurrentPassword,dto.NewPassword);
            if(!result.Succeeded)
                return BadRequest(result.Message);
            return Ok(result.Message);


        }

        [HttpGet("forget-password")]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {
            if(email==null)
                return BadRequest("Email is required");

            bool succeeded =await _authService.GenerateOtpAsync(email);
            if (!succeeded)
                return BadRequest("Invalid email");

            return Ok("A 6 digits code has been sent to you");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _authService.ResetPasswordWithOtpAsync(dto.Email, dto.Otp, dto.NewPassword);
            if (result.Succeeded)
                return Ok(result.Message);

            return NotFound(result);

        }


        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Refresh token is required");

            var result = await _authService.RefreshAsync(token);

            if (!result.IsAuthenticated)
                return Unauthorized(result.Message);

            return Ok(result);
        }
    }
}
