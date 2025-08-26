using WebApp.Application.Dtos.Auth;

namespace WebApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string email ,string password);
        Task<bool> RegisterAsync(RegisterDto registerDto);
    }
}
