using WebApp.Core.entities;
using WebApp.Core.interfaces;
using WebApp.Infrastructure.persistence;

namespace WebApp.Infrastructure.repositories
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly AppDbContext _context;


        public UnitOfWork(AppDbContext context,
            IGenericRepo<Product> productRepo,
            IGenericRepo<Category> categoryRepo
           )
        {
            _context = context;
            ProductRepo = productRepo;
            CategoryRepo = categoryRepo;
        }


        public IGenericRepo<Product> ProductRepo { get; private set; } = null!;
        public IGenericRepo<Category> CategoryRepo { get; private set; } = null!;
       
        public void Dispose() => _context.Dispose();
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    }
}

