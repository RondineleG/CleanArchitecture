using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.DeleteProductById;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using Application.Features.Products.Queries.GetProductById;
using Application.Interfaces.Validations;

using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using WebApi.Controllers;

public class ProductController : BaseApiController
{
    public ProductController(
        IRequestHandler<CreateProductCommand, int> createProductService,
        IRequestHandler<DeleteProductByIdCommand, bool> deleteProductService,
        IRequestHandler<GetAllProductsQuery, IEnumerable<Product>> getAllProductsService,
        IRequestHandler<GetProductByIdQuery, int> getProductByIdService,
        IRequestHandler<UpdateProductCommand, int> updateProductService)
    {
        _createProductService = createProductService;
        _deleteProductService = deleteProductService;
        _getAllProductsService = getAllProductsService;
        _getProductByIdService = getProductByIdService;
        _updateProductService = updateProductService;
    }

    private readonly IRequestHandler<CreateProductCommand, int> _createProductService;

    private readonly IRequestHandler<DeleteProductByIdCommand, bool> _deleteProductService;

    private readonly IRequestHandler<GetAllProductsQuery, IEnumerable<Product>> _getAllProductsService;

    private readonly IRequestHandler<GetProductByIdQuery, int> _getProductByIdService;

    private readonly IRequestHandler<UpdateProductCommand, int> _updateProductService;

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteProductByIdCommand { Id = id };
        var result = await _deleteProductService.ExecuteAsync(command, default).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllProductsParameter filter)
    {
        var query = new GetAllProductsQuery { PageNumber = filter.PageNumber, PageSize = filter.PageSize };
        var result = await _getAllProductsService.ExecuteAsync(query, default).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var query = new GetProductByIdQuery { Id = id };
        var result = await _getProductByIdService.ExecuteAsync(query, default).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await _createProductService.ExecuteAsync(command, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Put(int id, UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _updateProductService.ExecuteAsync(command, default).ConfigureAwait(false);
        return Ok(result);
    }
}