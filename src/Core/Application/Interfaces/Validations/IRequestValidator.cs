using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Validations;

public interface IRequestValidator<TRequest>
{
    Task<IEnumerable<string>> ValidateAsync(TRequest request, CancellationToken cancellationToken);
}