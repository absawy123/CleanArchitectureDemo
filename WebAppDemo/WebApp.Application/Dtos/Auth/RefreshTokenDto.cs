namespace WebApp.Application.Dtos.Auth
{
    public class RefreshTokenDto
    {
        public string? RefreshToken { get; set; }
        public string? JwtToken { get; set; }
    }
}
