// FlowTask.Shared/Results/ErrorCodes.cs
namespace FlowTask.Shared.Results;

/// <summary>
/// Machine-readable error codes shared between backend and frontend.
/// Kept in FlowTask.Shared so both Application and Api layers reference the
/// same constants without pulling in each other.
///
/// In Api files that also use HotChocolate, alias this class to avoid the
/// name clash with HotChocolate.ErrorCodes:
///   using AppErrorCodes = FlowTask.Shared.Results.ErrorCodes;
/// </summary>
public static class ErrorCodes
{
    // ── HTTP-level ──────────────────────────────────────────────────────
    public const string ValidationError = "VALIDATION_ERROR";
    public const string Conflict = "CONFLICT";
    public const string NotFound = "NOT_FOUND";
    public const string Unauthorized = "UNAUTHORIZED";
    public const string Forbidden = "FORBIDDEN";
    public const string Unexpected = "UNEXPECTED_ERROR";

    // ── Auth ────────────────────────────────────────────────────────────
    public const string InvalidCredentials = "INVALID_CREDENTIALS";
    public const string AccountLocked = "ACCOUNT_LOCKED";
    public const string EmailAlreadyExists = "EMAIL_EXISTS";
    public const string TokenExpired = "TOKEN_EXPIRED";

    // ── Field-level validation rules ────────────────────────────────────
    public const string Required = "REQUIRED";
    public const string MinLength = "MIN_LENGTH";
    public const string MaxLength = "MAX_LENGTH";
    public const string EmailFormat = "EMAIL_FORMAT";
    public const string Pattern = "PATTERN";

    // ── Domain resources ────────────────────────────────────────────────
    public const string WorkspaceNotFound = "WORKSPACE_NOT_FOUND";
    public const string TaskNotFound = "TASK_NOT_FOUND";
    public const string SpaceNotFound = "SPACE_NOT_FOUND";
    public const string ListNotFound = "LIST_NOT_FOUND";
    public const string UserNotFound = "USER_NOT_FOUND";
    public const string BadRequest = "BAD_REQUEST";
}