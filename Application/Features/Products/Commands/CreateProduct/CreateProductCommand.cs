using Application.Behaviours;
using Application.Interfaces.Repositories;
using Application.Wrappers;

using AutoMapper;

using Domain.Entities;

using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand
{
    public string Barcode { get; set; }

    public string Description { get; set; }

    public string Name { get; set; }

    public decimal Rate { get; set; }
}

public class CreateProductService
{
    public CreateProductService(
        IProductRepositoryAsync productRepository,
        IMapper mapper,
        IRequestPipelineExecutor pipelineExecutor)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _pipelineExecutor = pipelineExecutor;
    }

    private readonly IMapper _mapper;

    private readonly IRequestPipelineExecutor _pipelineExecutor;

    private readonly IProductRepositoryAsync _productRepository;

    public Task<Response<int>> CreateAsync(CreateProductCommand request, CancellationToken cancellationToken)
    {
        return _pipelineExecutor.ExecuteAsync(
            request,
            next: () => HandleAsync(request, cancellationToken),
            cancellationToken
        );
    }

    private async Task<Response<int>> HandleAsync(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = _mapper.Map<Product>(request);
        await _productRepository.AddAsync(product);
        return new Response<int>(product.Id);
    }
}