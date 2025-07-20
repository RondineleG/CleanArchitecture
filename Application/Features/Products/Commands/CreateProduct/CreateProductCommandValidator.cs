using Application.Behaviours;
using Application.Interfaces.Repositories;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : IRequestValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IProductRepositoryAsync productRepository)
    {
        _productRepository = productRepository;
    }

    private readonly IProductRepositoryAsync _productRepository;

    public async Task<IEnumerable<string>> ValidateAsync(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Barcode))
            errors.Add("Barcode is required.");
        else if (request.Barcode.Length > 50)
            errors.Add("Barcode must not exceed 50 characters.");
        else if (!await _productRepository.IsUniqueBarcodeAsync(request.Barcode))
            errors.Add("Barcode already exists.");

        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add("Name is required.");
        else if (request.Name.Length > 50)
            errors.Add("Name must not exceed 50 characters.");

        return errors;
    }
}