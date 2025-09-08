using WebApp.Application.Common;
using WebApp.Application.Dtos.Auth;

namespace WebApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthTokensDto>> LoginAsync(string email ,string password);
        Task<bool> RegisterAsync(RegisterDto registerDto);

        Task<Result> ChangePasswordAsync(string email, string currentPassword, string newPassword);

        Task<Result> GenerateOtpAsync(string email);

        Task<Result> ResetPasswordWithOtpAsync(string email, string otp, string newPassword);

        Task<Result<AuthTokensDto>> RefreshAsync(string token);


    }
}
