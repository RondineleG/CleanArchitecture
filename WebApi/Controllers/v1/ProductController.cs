using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.DeleteProductById;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using Application.Features.Products.Queries.GetProductById;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

using WebApi.Controllers;

public class ProductController : BaseApiController
{
    public ProductController(
        CreateProductService createProductService,
        DeleteProductByIdService deleteProductService,
        UpdateProductService updateProductService,
        GetAllProductsService getAllProductsService,
        GetProductByIdService getProductByIdService)
    {
        _createProductService = createProductService;
        _deleteProductService = deleteProductService;
        _updateProductService = updateProductService;
        _getAllProductsService = getAllProductsService;
        _getProductByIdService = getProductByIdService;
    }

    private readonly CreateProductService _createProductService;

    private readonly DeleteProductByIdService _deleteProductService;

    private readonly GetAllProductsService _getAllProductsService;

    private readonly GetProductByIdService _getProductByIdService;

    private readonly UpdateProductService _updateProductService;

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteProductByIdCommand { Id = id };
        var result = await _deleteProductService.ExecuteAsync(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllProductsParameter filter)
    {
        var query = new GetAllProductsQuery { PageNumber = filter.PageNumber, PageSize = filter.PageSize };
        var result = await _getAllProductsService.ExecuteAsync(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var query = new GetProductByIdQuery { Id = id };
        var result = await _getProductByIdService.ExecuteAsync(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await _createProductService.CreateAsync(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Put(int id, UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _updateProductService.ExecuteAsync(command);
        return Ok(result);
    }
}