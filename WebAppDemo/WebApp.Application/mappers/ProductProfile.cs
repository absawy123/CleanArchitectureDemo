using AutoMapper;
using WebApp.Application.dtos.productDtos;
using WebApp.Core.entities;

namespace WebApp.Application.mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }

    }
}
