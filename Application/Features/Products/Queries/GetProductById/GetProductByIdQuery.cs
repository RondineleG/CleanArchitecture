using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Validations;
using Application.Wrappers;

using Domain.Entities;

using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery
{
    public int Id { get; set; }
}

public class GetProductByIdService : IRequestHandler<GetProductByIdQuery, Response<Product>>
{
    public GetProductByIdService(IProductRepositoryAsync productRepository, IRequestPipelineExecutor pipelineExecutor)
    {
        _productRepository = productRepository;
        _pipelineExecutor = pipelineExecutor;
    }

    private readonly IRequestPipelineExecutor _pipelineExecutor;

    private readonly IProductRepositoryAsync _productRepository;

    public Task<Response<Product>> ExecuteAsync(GetProductByIdQuery request, CancellationToken cancellationToken = default)
    {
        return _pipelineExecutor.ExecuteAsync(
            request,
            next: () => HandleAsync(request, cancellationToken),
            cancellationToken
        );
    }

    private async Task<Response<Product>> HandleAsync(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(query.Id);
        return product == null
            ? throw new ApiException("Product Not Found.")
            : new Response<Product>(product);
    }
}