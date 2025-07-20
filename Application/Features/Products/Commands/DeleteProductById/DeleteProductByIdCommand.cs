using Application.Behaviours;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;

using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.DeleteProductById;

public class DeleteProductByIdCommand
{
    public int Id { get; set; }
}

public class DeleteProductByIdService : IRequestHandler<DeleteProductByIdCommand, Response<int>>
{
    public DeleteProductByIdService(
        IProductRepositoryAsync productRepository,
        IRequestPipelineExecutor pipelineExecutor)
    {
        _productRepository = productRepository;
        _pipelineExecutor = pipelineExecutor;
    }

    private readonly IRequestPipelineExecutor _pipelineExecutor;

    private readonly IProductRepositoryAsync _productRepository;

    public Task<Response<int>> ExecuteAsync(DeleteProductByIdCommand request, CancellationToken cancellationToken = default)
    {
        return _pipelineExecutor.ExecuteAsync(
            request,
            next: () => HandleAsync(request, cancellationToken),
            cancellationToken
        );
    }

    private async Task<Response<int>> HandleAsync(DeleteProductByIdCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id);
        if (product == null)
        {
            throw new ApiException("Product Not Found.");
        }

        await _productRepository.DeleteAsync(product);
        return new Response<int>(product.Id);
    }
}