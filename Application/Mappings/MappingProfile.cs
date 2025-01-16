using Application.Handlers.Commands.Products.CreateProduct;
using Application.Handlers.Commands.Products.UpdateProduct;
using Application.Handlers.Queries.Products.GetProductsList;
using AutoMapper;
using Domain.Entities;
using System.Reflection;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, GetProductsListViewModel>()
                .ForMember(dest => dest.TitleAr, opt => opt.MapFrom(src => src.TitleAr))
                .ForMember(dest => dest.TitleEn, opt => opt.MapFrom(src => src.TitleEn))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
            CreateMap<CreateProductCommand, Product>(); 
            CreateMap<UpdateProductCommand, Product>();

            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            dynamic types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }

    }
}
