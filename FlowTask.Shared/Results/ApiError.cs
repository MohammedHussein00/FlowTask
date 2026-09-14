// FlowTask.Shared/Results/ApiError.cs
namespace FlowTask.Shared.Results;

/// <summary>
/// The single error envelope returned by every failed operation,
/// whether it originates from REST middleware or GraphQL error filter.
///
/// JSON shape (always — errors is never null, just empty):
/// {
///   "code":    "VALIDATION_ERROR",
///   "message": "One or more validation errors occurred.",
///   "errors": [
///     { "field": "email",    "message": "Email is required.",  "code": "REQUIRED"    },
///     { "field": "password", "message": "Min 8 characters.",   "code": "MIN_LENGTH"  }
///   ]
/// }
/// </summary>
public sealed class ApiError
{
    /// <summary>Machine-readable error code (e.g. "VALIDATION_ERROR", "EMAIL_EXISTS").</summary>
    public string Code { get; init; } = ErrorCodes.Unexpected;

    /// <summary>Human-readable summary — safe to show in a toast or alert.</summary>
    public string Message { get; init; } = "An unexpected error occurred.";

    /// <summary>Per-field detail. Always present; empty list for non-validation errors.</summary>
    public IReadOnlyList<ApiFieldError> Errors { get; init; } = [];

    // ── Static factories ─────────────────────────────────────────────────

    public static ApiError Validation(string message, IReadOnlyList<ApiFieldError> errors) => new()
    {
        Code = ErrorCodes.ValidationError,
        Message = message,
        Errors = errors,
    };

    public static ApiError Conflict(string message) => new()
    {
        Code = ErrorCodes.Conflict,
        Message = message,
        Errors = [],
    };

    public static ApiError NotFound(string message) => new()
    {
        Code = ErrorCodes.NotFound,
        Message = message,
        Errors = [],
    };

    public static ApiError Unauthorized(string message = "You are not authorized.") => new()
    {
        Code = ErrorCodes.Unauthorized,
        Message = message,
        Errors = [],
    };

    public static ApiError Forbidden(string message = "You do not have permission to perform this action.") => new()
    {
        Code = ErrorCodes.Forbidden,
        Message = message,
        Errors = [],
    };

    public static ApiError Unexpected(string message = "An unexpected error occurred.") => new()
    {
        Code = ErrorCodes.Unexpected,
        Message = message,
        Errors = [],
    };
}

/// <summary>Per-field detail entry inside <see cref="ApiError.Errors"/>.</summary>
public sealed class ApiFieldError
{
    /// <summary>camelCase field path matching the frontend form control name (e.g. "email").</summary>
    public string Field { get; init; } = default!;

    /// <summary>Human-readable message for this specific field.</summary>
    public string Message { get; init; } = default!;

    /// <summary>Machine-readable rule code (e.g. "REQUIRED", "MIN_LENGTH", "EMAIL_EXISTS").</summary>
    public string Code { get; init; } = default!;

    public ApiFieldError() { }

    public ApiFieldError(string field, string message, string code)
    {
        Field = field;
        Message = message;
        Code = code;
    }
}