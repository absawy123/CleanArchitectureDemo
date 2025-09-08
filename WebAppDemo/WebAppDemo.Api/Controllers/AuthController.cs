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
        public async Task<IActionResult> LoginAsync(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _authService.LoginAsync(dto.Email, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);

        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var isSuccessfull = await _authService.RegisterAsync(registerDto);
            if (isSuccessfull)
                return Ok();

            return BadRequest("Can not register.");
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _authService.ChangePasswordAsync(dto.Email, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);


        }

        [HttpGet("forget-password")]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {
            if (email == null)
                return BadRequest("Email is required");

            var result = await _authService.GenerateOtpAsync(email);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _authService.ResetPasswordWithOtpAsync(dto.Email, dto.Otp, dto.NewPassword);
            if (result.Succeeded)
                return Ok(result);

            return NotFound(result);

        }


        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Refresh token is required");

            var result = await _authService.RefreshAsync(token);
            if (!result.Succeeded)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}
