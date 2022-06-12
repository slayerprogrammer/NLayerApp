using AutoMapper;
using NLayer.Core.DTOs.Category;
using NLayer.Core.DTOs.Custom;
using NLayer.Core.DTOs.Product;
using NLayer.Core.DTOs.ProductFeature;
using NLayer.Core.Models;

namespace NLayer.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            //CreateMap metodu ile entity ve dtoları eşleştirip mapliyoruz
            //ReversMap komutu ise bu mappingin iki yönlü olduğunu bildiriyor.

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, ProductWithCategoryDto>();
            CreateMap<Category, CategoryWithProductsDto>();

        }
    }
}
