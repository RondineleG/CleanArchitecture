using Application.Interfaces.Repositories;

using FluentValidation;

using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IProductRepositoryAsync productRepository;

    public CreateProductCommandValidator(IProductRepositoryAsync productRepository)
    {
        this.productRepository = productRepository;

        _ = RuleFor(p => p.Barcode)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
            .MustAsync(IsUniqueBarcode).WithMessage("{PropertyName} already exists.");

        _ = RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

    }

    private async Task<bool> IsUniqueBarcode(string barcode, CancellationToken cancellationToken)
    {
        return await productRepository.IsUniqueBarcodeAsync(barcode);
    }
}
