using Application.Interfaces.Repositories;

using Domain.Entities;

using Infrastructure.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories;

public class ProductRepositoryAsync : GenericRepositoryAsync<Product>, IProductRepositoryAsync
{
    public ProductRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _products = dbContext.Set<Product>();
    }

    private readonly DbSet<Product> _products;

    public Task<bool> IsUniqueBarcodeAsync(string barcode)
    {
        return _products
            .AllAsync(p => p.Barcode != barcode);
    }
}