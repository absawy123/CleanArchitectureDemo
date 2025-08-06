using AutoMapper;
using System.Linq.Expressions;
using WebApp.Application.dtos.categoryDtos;
using WebApp.Core.entities;
using WebApp.Core.interfaces;

namespace WebApp.Application.services
{
    public class CategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task AddAsync(AddCategorytDto dto)
        {
            var category = _mapper.Map<AddCategorytDto, Category>(dto);
            await _unitOfWork.CategoryRepo.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Category> GetByIdAsync(int id) =>  await _unitOfWork.CategoryRepo.GetAsync(c => c.Id == id);
       

        public async Task<IEnumerable<ReadCategoryDto>> GetAllAsync(Expression<Func<Category, bool>> filter = null!,
            bool isTracked = true, int pageSize = 0, int pageNumber = 0, params Expression<Func<Category, object>>[] includes)
        {
            var categories = await _unitOfWork.CategoryRepo.GetAllAsync(filter: filter, isTracked: isTracked, pageSize: pageSize,
              pageNumber: pageNumber, includes: includes);
            var categoryDtos = new List<ReadCategoryDto>();

            foreach (var category in categories)
            {
                var categoryDto = _mapper.Map<Category, ReadCategoryDto>(category);
                categoryDtos.Add(categoryDto);
            }
            return categoryDtos;
        }


        public async Task UpdateAsync(UpdateCategoryDto dto)
        {
            var category = _mapper.Map<UpdateCategoryDto, Category>(dto);
            _unitOfWork.CategoryRepo.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepo.GetAsync(c => c.Id == id);
            _unitOfWork.CategoryRepo.Delete(category);
            await _unitOfWork.SaveChangesAsync();
        }


    }
}
