using AutoMapper;
using WebApp.Application.dtos.categoryDtos;
using WebApp.Core.entities;

namespace WebApp.Application.mappers
{
    public class CategoryProfile :Profile
    {
        public CategoryProfile()
        {
            CreateMap<AddCategorytDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
        }
    }
}
