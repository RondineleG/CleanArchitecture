using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Validations;

public interface IRequestHandler<TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}