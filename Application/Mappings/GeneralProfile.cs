using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Queries.GetAllProducts;

using AutoMapper;

using Domain.Entities;

namespace Application.Mappings;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        _ = CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
        _ = CreateMap<CreateProductCommand, Product>();
        _ = CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
    }
}