using WebApp.Core.entities;

namespace WebApp.Core.interfaces
{
    public interface IUnitOfWork :IDisposable
    {
        public IGenericRepo<Product> ProductRepo { get; }
        public IGenericRepo<Category> CategoryRepo { get; }
        Task<int> SaveChangesAsync();
        
    }
}
