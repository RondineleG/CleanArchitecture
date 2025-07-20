using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviours;

public class ValidationBehaviour : IRequestPipelineExecutor
{
    public ValidationBehaviour(IServiceProvider provider)
    {
        _provider = provider;
    }

    private readonly IServiceProvider _provider;

    public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default)
    {
        var validators = _provider.GetServices<IRequestValidator<TRequest>>();
        var pipeline = new RequestValidationPipeline<TRequest, TResponse>(validators, next);
        return await pipeline.ExecuteAsync(request, cancellationToken);
    }
}

public interface IRequestPipelineExecutor
{
    Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default);
}

public interface IRequestValidator<TRequest>
{
    Task<IEnumerable<string>> ValidateAsync(TRequest request, CancellationToken cancellationToken);
}

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