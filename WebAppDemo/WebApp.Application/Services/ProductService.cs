using AutoMapper;
using System.Linq.Expressions;
using WebApp.Application.dtos.productDtos;
using WebApp.Application.Interfaces;
using WebApp.Core.entities;
using WebApp.Core.interfaces;

namespace WebApp.Application.services
{
    public class ProductService :IProductService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<GetProductDto> AddAsync(ProductDto dto)
        {
            var product = _mapper.Map<ProductDto, Product>(dto);
            await _unitOfWork.ProductRepo.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            var productDto = _mapper.Map<GetProductDto>(product);
            return productDto;
        }

        public async Task<GetProductDto> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepo.GetAsync(p => p.Id == id);
            var dto = _mapper.Map<Product ,GetProductDto>(product);
            return dto;
        }

        public async Task<IEnumerable<GetProductDto>> GetAllAsync(Expression<Func<Product, bool>> filter = null!,
            bool isTracked = true, int pageSize = 0, int pageNumber = 0, params Expression<Func<Product, object>>[] includes)
        {
            var products = await _unitOfWork.ProductRepo.GetAllAsync(filter: filter, isTracked: isTracked, pageSize: pageSize,
              pageNumber: pageNumber, includes: includes);
            var productDtos = new List<GetProductDto>();

            foreach (var product in products)
            {
                var productDto = _mapper.Map<Product, GetProductDto>(product);
                productDtos.Add(productDto);
            }
            return productDtos;
        }


        public async Task UpdateAsync(int id, ProductDto dto)
        {
            var exisiting =await _unitOfWork.ProductRepo.GetAsync(p => p.Id==id);
            var product = _mapper.Map(dto,exisiting);
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
