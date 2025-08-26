using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApp.Core.entities;
using WebApp.Infrastructure.Identity;

namespace WebApp.Infrastructure.persistence
{
    public class AppDbContext :IdentityDbContext<ApplicationUser> 
    {


        public AppDbContext()
        {
            
        }

        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

      




    }
}
