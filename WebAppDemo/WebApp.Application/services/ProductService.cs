using AutoMapper;
using System.Linq.Expressions;
using WebApp.Application.dtos.productDtos;
using WebApp.Core.entities;
using WebApp.Core.interfaces;

namespace WebApp.Application.services
{
    public class ProductService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task AddAsync(AddProductDto dto)
        {
            var product = _mapper.Map<AddProductDto, Product>(dto);
            await _unitOfWork.ProductRepo.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Product> GetByIdAsync(int id) => await _unitOfWork.ProductRepo.GetAsync(p => p.Id == id);
           

        public async Task<IEnumerable<ReadProductDto>> GetAllAsync(Expression<Func<Product, bool>> filter = null!,
            bool isTracked = true, int pageSize = 0, int pageNumber = 0, params Expression<Func<Product, object>>[] includes)
        {
            var products = await _unitOfWork.ProductRepo.GetAllAsync(filter: filter, isTracked: isTracked, pageSize: pageSize,
              pageNumber: pageNumber, includes: includes);
            var productDtos = new List<ReadProductDto>();

            foreach (var product in products)
            {
                var productDto = _mapper.Map<Product, ReadProductDto>(product);
                productDtos.Add(productDto);
            }
            return productDtos;
        }


        public async Task UpdateAsync(UpdateProductDto dto)
        {
            var product = _mapper.Map<UpdateProductDto, Product>(dto);
            _unitOfWork.ProductRepo.Update(product);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task RemoveAsync(int id)
        {
            var product = await _unitOfWork.ProductRepo.GetAsync(p => p.Id == id);
            _unitOfWork.ProductRepo.Delete(product);
            await _unitOfWork.SaveChangesAsync();

        }


    }
}
