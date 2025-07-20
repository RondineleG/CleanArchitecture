using Application.Behaviours;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;

using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand
{
    public string Description { get; set; }

    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Rate { get; set; }
}

public class UpdateProductService
{
    public UpdateProductService(IProductRepositoryAsync productRepository, IRequestPipelineExecutor pipelineExecutor)
    {
        _productRepository = productRepository;
        _pipelineExecutor = pipelineExecutor;
    }

    private readonly IRequestPipelineExecutor _pipelineExecutor;

    private readonly IProductRepositoryAsync _productRepository;

    public Task<Response<int>> ExecuteAsync(UpdateProductCommand request, CancellationToken cancellationToken = default)
    {
        return _pipelineExecutor.ExecuteAsync(
            request,
            next: () => HandleAsync(request, cancellationToken),
            cancellationToken
        );
    }

    private async Task<Response<int>> HandleAsync(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id);
        if (product == null)
        {
            throw new ApiException("Product Not Found.");
        }

        product.Name = command.Name;
        product.Rate = command.Rate;
        product.Description = command.Description;

        await _productRepository.UpdateAsync(product);
        return new Response<int>(product.Id);
    }
}