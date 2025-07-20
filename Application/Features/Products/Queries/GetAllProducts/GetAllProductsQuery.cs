using Application.Behaviours;
using Application.Interfaces.Repositories;
using Application.Wrappers;

using AutoMapper;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQuery
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}

public class GetAllProductsService
{
    public GetAllProductsService(IProductRepositoryAsync productRepository, IMapper mapper, IRequestPipelineExecutor pipelineExecutor)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _pipelineExecutor = pipelineExecutor;
    }

    private readonly IMapper _mapper;

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
        var filter = _mapper.Map<GetAllProductsParameter>(request);
        var products = await _productRepository.GetPagedReponseAsync(filter.PageNumber, filter.PageSize);
        var productViewModels = _mapper.Map<IEnumerable<GetAllProductsViewModel>>(products);
        return new PagedResponse<IEnumerable<GetAllProductsViewModel>>(productViewModels, filter.PageNumber, filter.PageSize);
    }
}