using WebApp.Core.entities;
using WebApp.Infrastructure.persistence;

namespace WebApp.Infrastructure.repositories
{
    public class CategoryRepo : GenericRepo<Category>
    {
        private readonly AppDbContext _context;
        public CategoryRepo(AppDbContext context) : base(context)
        {
            _context = context;

        }
    }
}
