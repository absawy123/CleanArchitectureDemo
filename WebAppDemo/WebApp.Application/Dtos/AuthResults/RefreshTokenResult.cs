namespace WebApp.Application.Dtos.AuthResults
{
    public class RefreshTokenResult
    {
        public string? Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? RefreshToken { get; set; }
        public string? JwtToken { get; set; }
    }
}
