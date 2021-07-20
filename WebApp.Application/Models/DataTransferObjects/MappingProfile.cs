using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Application.Models.DataTransferObjects.Incoming.Categories;
using WebApp.Application.Models.DataTransferObjects.Incoming.Products;
using WebApp.Application.Models.DataTransferObjects.Incoming.Providers;
using WebApp.Application.Models.DataTransferObjects.Incoming.Users;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Categories;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Products;
using WebApp.Application.Models.DataTransferObjects.Outgoing.Providers;
using WebApp.Data.Entities;

namespace WebApp.Application.Models.DataTransferObjects
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, User>();

            CreateMap<Product, ProductFullInfoDto>()
                .ForMember(p => p.Category, opt => opt.MapFrom(x => x.Category.Name))
                .ForMember(p => p.Provider, opt => opt.MapFrom(x => x.Provider.Name));
            CreateMap<ProductAddDto, Product>()
                .ForMember(p => p.Category, opt => opt.Ignore())
                .ForMember(p => p.Provider, opt => opt.Ignore());
            CreateMap<Product, ProductAddDto>();
            CreateMap<ProductUpdateDto, Product>()
                .ForMember(p => p.Category, opt => opt.Ignore())
                .ForMember(p => p.Provider, opt => opt.Ignore());
            CreateMap<Product, ProductUpdateDto>();

            CreateMap<Category, CategoryFullInfoDto>();
            CreateMap<CategoryAddDto, Category>()
                .ForMember(p => p.Products, opt => opt.Ignore());
            CreateMap<Category, CategoryAddDto>();
            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(p => p.Products, opt => opt.Ignore());
            CreateMap<Category, CategoryUpdateDto>();

            CreateMap<Provider, ProviderFullInfoDto>();
            CreateMap<ProviderAddDto, Provider>()
                .ForMember(p => p.Products, opt => opt.Ignore());
            CreateMap<Provider, ProviderAddDto>();
            CreateMap<ProviderUpdateDto, Provider>()
                .ForMember(p => p.Products, opt => opt.Ignore());
            CreateMap<Provider, ProviderUpdateDto>();
        }
    }
}
