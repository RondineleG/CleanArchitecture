using Application.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using System;
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