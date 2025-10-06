using Asclepius.Auth.Data.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Asclepius.Auth.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Asclepius.Auth.Api.ExceptionHandlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is DomainException domainException)
        {
            var statusCode = domainException.StatusCode;
            
            logger.LogError(exception, exception.Message);
            
            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = "Ошибка обработки запроса",
                Detail = domainException.Message 
            };
            
            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
        else if (exception is DataException dataException)
        {
            var statusCode = dataException.StatusCode;
            
            logger.LogError(exception, exception.Message);
            
            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = "Ошибка обработки запроса",
                Detail = dataException.Message 
            };
            
            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
        else
        {
            logger.LogError(exception, "Непредвиденная системная ошибка: {Message}", exception.Message);
            
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Внутренняя ошибка сервера",
                Detail = "Произошла непредвиденная ошибка. Пожалуйста, попробуйте позже."
            };
            
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true; 
        }
    }
}