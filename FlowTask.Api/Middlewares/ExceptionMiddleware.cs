// FlowTask.Api/Middlewares/ExceptionMiddleware.cs
namespace FlowTask.Api.Middlewares;

using FlowTask.Api.ErrorHandling;
using FlowTask.Application.Exceptions;
using FlowTask.Shared.Localization;
using FlowTask.Shared.Results;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Catches every unhandled exception and writes a consistent <see cref="ApiError"/>
/// JSON body. HTTP status is derived from the exception type.
///
/// Response shape (always):
/// {
///   "code":    "VALIDATION_ERROR",
///   "message": "One or more validation errors occurred.",
///   "errors":  [ { "field": "email", "message": "...", "code": "REQUIRED" } ]
/// }
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false,
    };

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var loc = context.RequestServices.GetRequiredService<ILocalizationService>();

        var (statusCode, apiError) = ex switch
        {
            AppException appEx => (HttpStatusCodeFor(appEx), AppExceptionResponseFactory.Build(appEx, loc)),
            _ => (HttpStatusCode.InternalServerError, ApiError.Unexpected())
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(apiError, _jsonOptions));
    }

    private static HttpStatusCode HttpStatusCodeFor(AppException ex) => ex switch
    {
        ValidationException => HttpStatusCode.BadRequest,
        BadRequestException => HttpStatusCode.BadRequest,
        InvalidCredentialsException => HttpStatusCode.Unauthorized,
        UnauthenticatedException => HttpStatusCode.Unauthorized,
        UnauthorizedException => HttpStatusCode.Unauthorized,
        TokenExpiredException => HttpStatusCode.Unauthorized,
        ForbiddenException => HttpStatusCode.Forbidden,
        NotFoundException => HttpStatusCode.NotFound,
        ConflictException => HttpStatusCode.Conflict,
        EmailAlreadyExistsException => HttpStatusCode.Conflict,
        _ => HttpStatusCode.InternalServerError,
    };
}