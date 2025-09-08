using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApp.Application.Common;
using WebApp.Application.Dtos.Auth;
using WebApp.Application.Interfaces;
using WebApp.Core.Entities;

namespace WebApp.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;


        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IMemoryCache cache,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _cache = cache;
            _emailService = emailService;
        }




        public async Task<Result<AuthTokensDto>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<AuthTokensDto>.Failure("Invalid email");
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                return Result<AuthTokensDto>.Failure("Invalid password");

            var accessToken = await GenerateTokenAsync(user);
            var newRefreshToken = GenerateRefreshToken(user.Id);

            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            return Result<AuthTokensDto>.Success(new AuthTokensDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            });

        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser { UserName = registerDto.UserName, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            return result.Succeeded;
        }


        public async Task<Result> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure("Invalid email");

            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!isCorrectPassword)
                return Result.Failure("Invalid password");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
                return Result.Failure("Cant change password", result.Errors.Select(e => e.Description).ToList());


            return Result.Success("Password has been changed successfully ");
        }


        public async Task<Result> GenerateOtpAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure("Invalid email");

            // Generate random 6-digit OTP
            var otp = new Random().Next(100000, 999999).ToString();

            _cache.Set($"OTP_{email}", otp, TimeSpan.FromMinutes(1));

            await _emailService.SendEmailAsync(email, "Password Reset code",
               $"Your code is {otp}");

            return Result.Success("Otp has been sent successfully");
        }

        public async Task<Result> ResetPasswordWithOtpAsync(string email, string otp, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure("Invalid email");

            if (!_cache.TryGetValue($"OTP_{email}", out string? cachedOtp) || cachedOtp != otp)
                return Result.Failure("Invalid otp");


            // 3. Generate password reset token
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!result.Succeeded)
                return Result.Failure("can not reset password", result.Errors.Select(e => e.Description).ToList());

            _cache.Remove($"OTP_{email}");

            return Result.Success("Password has been reset successfully");

        }

        // ask eng ahmed in this (performance of this query or better to use dbcontext to reach existedRefreshtoken first and then reach to user of this token)
        public async Task<Result<AuthTokensDto>> RefreshAsync(string token)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == token && r.ExpiresOn >= DateTime.UtcNow && r.RevokedOn == null));

            if (user == null)
                return Result<AuthTokensDto>.Failure("Invaild or expired token,Try to log in again");
              
            var oldToken = user.RefreshTokens.First(r => r.Token == token);
            oldToken.RevokedOn = DateTime.UtcNow;

            var accessToken = await GenerateTokenAsync(user);
            var newRefreshToken = GenerateRefreshToken(user.Id);

            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var refreshTokenDto = new AuthTokensDto()
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            };

            return Result<AuthTokensDto>.Success(refreshTokenDto, "Token refreshed successfully");
           
        }



        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var authClaims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private RefreshToken GenerateRefreshToken(string userId)
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow,
                UserId = userId
            };
        }


    }
}
