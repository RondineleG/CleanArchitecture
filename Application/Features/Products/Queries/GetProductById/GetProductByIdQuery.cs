using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;

using Domain.Entities;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<Response<Product>>
{
    public int Id { get; set; }
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<Product>>
    {
        private readonly IProductRepositoryAsync _productRepository;
        public GetProductByIdQueryHandler(IProductRepositoryAsync productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Response<Product>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            Product product = await _productRepository.GetByIdAsync(query.Id);
            return product == null ? throw new ApiException($"Product Not Found.") : new Response<Product>(product);
        }
    }
}
