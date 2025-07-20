using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IGenericRepositoryAsync<T, in TId> where T : class
{
    Task<T> AddAsync(T entity);

    Task DeleteAsync(T entity);

    Task<IReadOnlyList<T>> GetAllAsync();

    Task<T> GetByIdAsync(TId id);

    Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);

    Task UpdateAsync(T entity);
}

public interface IGenericRepositoryAsync<T> : IGenericRepositoryAsync<T, int> where T : class
{
}