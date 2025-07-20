using Application.Exceptions;
using Application.Wrappers;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Middlewares;

public class ErrorHandlerMiddleware
{
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private readonly RequestDelegate _next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";
            Response<string> responseModel = new() { Succeeded = false, Message = error?.Message };

            switch (error)
            {
                case Application.Exceptions.ApiException:
                // custom application error
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

                case ValidationException e:
                // custom application error
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel.Errors = e.Errors;
                break;

                case KeyNotFoundException:
                // not found error
                response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

                default:
                // unhandled error
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            }
            string result = JsonSerializer.Serialize(responseModel);

            await response.WriteAsync(result);
        }
    }
}