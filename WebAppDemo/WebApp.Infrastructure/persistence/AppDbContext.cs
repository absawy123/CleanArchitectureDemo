using Microsoft.EntityFrameworkCore;
using WebApp.Core.entities;

namespace WebApp.Infrastructure.persistence
{
    public class AppDbContext :DbContext 
    {


        public AppDbContext()
        {
            
        }

        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }




    }
}
