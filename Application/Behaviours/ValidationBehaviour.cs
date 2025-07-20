using FluentValidation;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviours;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            ValidationContext<TRequest> context = new(request);
            FluentValidation.Results.ValidationResult[] validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            List<FluentValidation.Results.ValidationFailure> failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                throw new Exceptions.ValidationException();
            }
        }
        return await next();
    }
}


public interface IRequestValidator<TRequest>
{
    Task<IEnumerable<string>> ValidateAsync(TRequest request, CancellationToken cancellationToken);
}

public class RequestValidationPipeline<TRequest, TResponse>
{
    private readonly IEnumerable<IRequestValidator<TRequest>> _validators;
    private readonly Func<Task<TResponse>> _next;

    public RequestValidationPipeline(IEnumerable<IRequestValidator<TRequest>> validators, Func<Task<TResponse>> next)
    {
        _validators = validators;
        _next = next;
    }

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

