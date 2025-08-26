namespace WebApp.Application.Dtos.Auth
{
    public class AuthTokensDto
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
