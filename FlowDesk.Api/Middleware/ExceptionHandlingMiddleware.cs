using FlowDesk.Api.Models;
using FlowDesk.Domain.Exceptions;
using Serilog;

namespace FlowDesk.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Timestamp = DateTime.UtcNow,
        };

        switch (exception)
        {
            case InvalidTaskException ite:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Code = ite.Code;
                response.Message = ite.Message;
                break;

            case TaskNotFoundException tnfe:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Code = tnfe.Code;
                response.Message = tnfe.Message;
                break;

            case ProjectNotFoundException pnfe:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Code = pnfe.Code;
                response.Message = pnfe.Message;
                break;

            case UserNotFoundException unfe:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Code = unfe.Code;
                response.Message = unfe.Message;
                break;

            case UserNotFoundByEmailException unfee:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Code = unfee.Code;
                response.Message = unfee.Message;
                break;

            case InvalidStatusTransitionException iste:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Code = iste.Code;
                response.Message = iste.Message;
                break;

            case DueDateInPastException dipe:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Code = dipe.Code;
                response.Message = dipe.Message;
                break;

            case InvalidPriorityException ipe:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Code = ipe.Code;
                response.Message = ipe.Message;
                break;

            case AccessDeniedException ade:
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                response.StatusCode = StatusCodes.Status403Forbidden;
                response.Code = ade.Code;
                response.Message = ade.Message;
                break;

            case DuplicateEmailException dee:
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                response.StatusCode = StatusCodes.Status409Conflict;
                response.Code = dee.Code;
                response.Message = dee.Message;
                break;

            case ValidationException ve:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Code = "VALIDATION_ERROR";
                response.Message = "One or more validation errors occurred.";
                response.Errors = ve.Errors;
                break;

            default:
                Log.Error(exception, "An unhandled exception occurred");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Code = "INTERNAL_SERVER_ERROR";
                response.Message = "An internal error occurred. Please try again later.";
                break;
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}

public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException(Dictionary<string, string[]> errors) : base("Validation failed")
    {
        Errors = errors;
    }
}
