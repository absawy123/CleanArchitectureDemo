namespace WebApp.Application.Dtos.Auth
{
    public class ResetPasswordDto
    {
        public string Email { get; set; } = default!;
        public string Otp { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
