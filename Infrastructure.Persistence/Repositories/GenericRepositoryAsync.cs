using Application.Interfaces;

using Infrastructure.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories;

public class GenericRepositoryAsync<T, TId> : IGenericRepositoryAsync<T, TId> where T : class
{
    private readonly ApplicationDbContext _dbContext;

    public GenericRepositoryAsync(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<T> GetByIdAsync(TId id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
    {
        return await _dbContext
            .Set<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        _ = await _dbContext.Set<T>().AddAsync(entity);
        _ = await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        _ = await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _ = _dbContext.Set<T>().Remove(entity);
        _ = await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext
             .Set<T>()
             .ToListAsync();
    }


}

public class GenericRepositoryAsync<T> : GenericRepositoryAsync<T, int>, IGenericRepositoryAsync<T> where T : class
{
    public GenericRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
