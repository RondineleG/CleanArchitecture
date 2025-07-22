using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces;

public class RequestValidationPipeline<TRequest, TResponse>
{
    public RequestValidationPipeline(IEnumerable<IRequestValidator<TRequest>> validators, Func<Task<TResponse>> next)
    {
        _validators = validators;
        _next = next;
    }

    private readonly Func<Task<TResponse>> _next;

    private readonly IEnumerable<IRequestValidator<TRequest>> _validators;

    public async Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        List<string> errors = [];

        foreach (IRequestValidator<TRequest> validator in _validators)
        {
            IEnumerable<string> result = await validator.ValidateAsync(request, cancellationToken);
            errors.AddRange(result);
        }

        if (errors.Count != 0)
        {
            throw new Exceptions.ValidationException(errors);
        }

        return await _next();
    }
}