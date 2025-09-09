using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

namespace WebApp.Core.Entities
{
    public class ApplicationUser :IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new Collection<RefreshToken>();
    }

}
