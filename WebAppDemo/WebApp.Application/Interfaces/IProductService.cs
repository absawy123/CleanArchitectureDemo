using System.Linq.Expressions;
using WebApp.Application.dtos.productDtos;
using WebApp.Core.entities;

namespace WebApp.Application.Interfaces
{
    public interface IProductService
    {
        Task AddAsync(ProductDto dto);
        Task<GetProductDto> GetByIdAsync(int id);
        Task<IEnumerable<GetProductDto>> GetAllAsync(Expression<Func<Product, bool>> filter = null!,
            bool isTracked = true, int pageSize = 0, int pageNumber = 0, params Expression<Func<Product, object>>[] includes);

        Task UpdateAsync(int id ,ProductDto dto);
        Task RemoveAsync(int id);

    }

}
