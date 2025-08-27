using WebApp.Application.Dtos.Auth;
using WebApp.Application.Dtos.AuthResults;

namespace WebApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthTokensDto?> LoginAsync(string email ,string password);
        Task<bool> RegisterAsync(RegisterDto registerDto);

        Task<ChangePasswordResult> ChangePasswordAsync(string email, string currentPassword, string newPassword);

        Task<bool> GenerateOtpAsync(string email);

        Task<ResetPasswordResult> ResetPasswordWithOtpAsync(string email, string otp, string newPassword);

        Task<RefreshTokenResult> RefreshAsync(string token);


    }
}
