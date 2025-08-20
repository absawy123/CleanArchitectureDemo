using WebApp.Core.entities;
using WebApp.Infrastructure.persistence;

namespace WebApp.Infrastructure.repositories
{
    public class ProductRepo :GenericRepo<Product> 
    {
        private readonly AppDbContext _context;
        public ProductRepo(AppDbContext context) : base(context)
        {
            _context = context;

        }
    }
}
