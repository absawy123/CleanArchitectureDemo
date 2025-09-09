using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using WebApp.Core.entities;
using WebApp.Core.Entities;

namespace WebApp.Infrastructure.persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {


        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            var clientRole = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Client", NormalizedName = "CLIENT" };
            var adminRole = new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" };
            modelBuilder.Entity<IdentityRole>().HasData(clientRole,adminRole);

            var hasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin",
                NormalizedUserName = "ADMIN@MAIL.COM",
                Email = "admin@mail.com",
                NormalizedEmail = "ADMIN@MAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var clientUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "client",
                NormalizedUserName = "CLIENT@MAIL.COM",
                Email = "client@mail.com",
                NormalizedEmail = "CLIENT@MAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "123456");
            clientUser.PasswordHash = hasher.HashPassword(clientUser, "123456");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser ,clientUser);
            
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = clientUser.Id, RoleId = clientRole.Id }, 
                new IdentityUserRole<string> { UserId = adminUser.Id, RoleId = adminRole.Id }
            );


        }






    }
}
