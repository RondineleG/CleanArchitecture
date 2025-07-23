using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Queries.GetAllProducts;

using Domain.Entities;

namespace Application.Mappings;

public static class MappingExtensions
{
    public static Product ToEntity(this GetAllProductsViewModel model)
    {
        return new Product
        {
            Id = model.Id,
            Name = model.Name,
            Barcode = model.Barcode,
            Description = model.Description,
            Rate = model.Rate
        };
    }

    public static Product ToEntity(this CreateProductCommand command)
    {
        return new Product
        {
            Name = command.Name,
            Barcode = command.Barcode,
            Description = command.Description,
            Rate = command.Rate
        };
    }

    public static GetAllProductsParameter ToParameter(this GetAllProductsQuery query)
    {
        return new GetAllProductsParameter
        {
            PageNumber = query.PageNumber < 1 ? 1 : query.PageNumber,
            PageSize = query.PageSize > 10 ? 10 : query.PageSize
        };
    }

    public static GetAllProductsViewModel ToViewModel(this Product product)
    {
        return new GetAllProductsViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Barcode = product.Barcode,
            Description = product.Description,
            Rate = product.Rate
        };
    }
}