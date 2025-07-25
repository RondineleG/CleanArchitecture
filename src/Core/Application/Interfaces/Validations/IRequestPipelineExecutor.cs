﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Validations;

public interface IRequestPipelineExecutor
{
    Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken = default);
}