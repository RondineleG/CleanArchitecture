using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Mappings;
using Application.Wrappers;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQuery
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}

public class GetAllProductsService : IRequestHandler<GetAllProductsQuery, PagedResponse<IEnumerable<GetAllProductsViewModel>>>
{
    public GetAllProductsService(IProductRepositoryAsync productRepository, IRequestPipelineExecutor pipelineExecutor)
    {
        _productRepository = productRepository;
        _pipelineExecutor = pipelineExecutor;
    }

    private readonly IRequestPipelineExecutor _pipelineExecutor;

    private readonly IProductRepositoryAsync _productRepository;

    public Task<PagedResponse<IEnumerable<GetAllProductsViewModel>>> ExecuteAsync(GetAllProductsQuery request, CancellationToken cancellationToken = default)
    {
        return _pipelineExecutor.ExecuteAsync(
            request,
            next: () => HandleAsync(request, cancellationToken),
            cancellationToken
        );
    }

    private async Task<PagedResponse<IEnumerable<GetAllProductsViewModel>>> HandleAsync(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var filter = request.ToParameter();
        var products = await _productRepository.GetPagedReponseAsync(filter.PageNumber, filter.PageSize);
        var productViewModels = products.Select(p => p.ToViewModel());
        return new PagedResponse<IEnumerable<GetAllProductsViewModel>>(productViewModels, filter.PageNumber, filter.PageSize);
    }
}