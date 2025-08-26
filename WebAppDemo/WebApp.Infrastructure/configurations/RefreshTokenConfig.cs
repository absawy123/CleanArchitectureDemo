using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp.Core.Entities;

namespace WebApp.Infrastructure.Configurations
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(r => new { r.Id, r.UserId });
            builder.HasOne(r => r.User).WithMany(a => a.RefreshTokens).HasForeignKey(r => r.UserId);
        }

    }
}
