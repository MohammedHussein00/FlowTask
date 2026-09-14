namespace FlowTask.Api.Extensions;

using System.Net;
using Microsoft.AspNetCore.Mvc;
using FlowTask.Api.ErrorHandling;
using FlowTask.Shared.Localization;
using FlowTask.Shared.Results;

public static class ResultExtensions
{
    public static HttpStatusCode ToHttpStatus(this ResultError e) => e.Kind switch
    {
        ErrorKind.Validation => HttpStatusCode.BadRequest,
        ErrorKind.BadRequest => HttpStatusCode.BadRequest,
        ErrorKind.Unauthorized => HttpStatusCode.Unauthorized,
        ErrorKind.Forbidden => HttpStatusCode.Forbidden,
        ErrorKind.NotFound => HttpStatusCode.NotFound,
        ErrorKind.Conflict => HttpStatusCode.Conflict,
        _ => HttpStatusCode.InternalServerError,
    };

    /// <summary>For controllers: 200 + ApiResponse.Ok, or {status} + ApiResponse.Fail.</summary>
    public static ActionResult<ApiResponse<T>> ToActionResult<T>(
        this Result<T> result, ILocalizationService loc)
    {
        if (result.IsSuccess)
            return new OkObjectResult(ApiResponse<T>.Ok(result.Value!));

        var apiError = LocalizedErrorFactory.Build(result.Error!, loc);
        return new ObjectResult(ApiResponse<T>.Fail(apiError))
        {
            StatusCode = (int)result.Error!.ToHttpStatus()
        };
    }
}