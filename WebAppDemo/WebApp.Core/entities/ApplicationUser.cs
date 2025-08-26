using Microsoft.AspNetCore.Identity;

namespace WebApp.Core.Entities
{
    public class ApplicationUser :IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }

}
