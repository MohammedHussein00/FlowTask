// FlowTask.Application/Exceptions/AppExceptions.cs
namespace FlowTask.Application.Exceptions;

// ─────────────────────────────────────────────────────────────────────────────
// Base — every exception carries a stable machine-readable Code plus a
// localization key/resource file/args, so the GraphQL/API error filter can
// build a client message in the request's language. DevMessage (optional) is
// for logs/telemetry only and should never be surfaced to the client.
//
// NOTE: ValidationException, ConflictException, ForbiddenException, and
// NotFoundException are intentionally NOT defined here — they already exist
// in their own files (Validationexception.cs, Conflictexception.cs,
// Forbiddenexception.cs, Notfoundexception.cs) using this exact base
// signature. Defining them again here would cause duplicate-definition
// errors, so this file only holds the base class plus the exceptions that
// don't have their own file yet.
// ─────────────────────────────────────────────────────────────────────────────

public abstract class AppException : Exception
{
    /// <summary>Stable machine-readable error code (e.g. "NOT_FOUND", "VALIDATION_ERROR").</summary>
    public string Code { get; }

    /// <summary>Resource key to look up in <see cref="ResourceFile"/> for the localized message.</summary>
    public string LocalizationKey { get; }

    /// <summary>Resource file/bundle the <see cref="LocalizationKey"/> lives in (default "Common").</summary>
    public string ResourceFile { get; }

    /// <summary>Format args substituted into the localized message template.</summary>
    public object[] Args { get; }

    /// <summary>Optional English-only diagnostic message for logs — never shown to the client.</summary>
    public string? DevMessage { get; }

    protected AppException(string code, string localizationKey, string resourceFile,
        object[]? args, string? devMessage)
        : base(devMessage ?? localizationKey)
    {
        Code = code;
        LocalizationKey = localizationKey;
        ResourceFile = resourceFile;
        Args = args ?? Array.Empty<object>();
        DevMessage = devMessage;
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 401 — generic unauthorized. Distinct from UnauthenticatedException (which
// specifically means "no / invalid / expired credentials"). Prefer
// UnauthenticatedException or ForbiddenException where they fit; use this
// only for 401 cases that are neither.
// ─────────────────────────────────────────────────────────────────────────────

public sealed class UnauthorizedException : AppException
{
    public const string Code = "UNAUTHORIZED";

    public UnauthorizedException(string localizationKey = "Unauthorized", string resourceFile = "Common",
        object[]? args = null, string? devMessage = null)
        : base(Code, localizationKey, resourceFile, args, devMessage) { }
}

public sealed class InvalidCredentialsException : AppException
{
    public const string Code = "INVALID_CREDENTIALS";

    public InvalidCredentialsException(string localizationKey = "InvalidCredentials", string resourceFile = "Common",
        object[]? args = null, string? devMessage = null)
        : base(Code, localizationKey, resourceFile, args, devMessage) { }
}

public sealed class TokenExpiredException : AppException
{
    public const string Code = "TOKEN_EXPIRED";

    public TokenExpiredException(string localizationKey = "TokenExpired", string resourceFile = "Common",
        object[]? args = null, string? devMessage = null)
        : base(Code, localizationKey, resourceFile, args, devMessage) { }
}

// ─────────────────────────────────────────────────────────────────────────────
// 409 — duplicate email registration. Specific enough to keep alongside the
// generic ConflictException (Conflictexception.cs); carries Email so the
// GraphQL layer can attach a field-level error to the "email" input without
// extra mapping.
// ─────────────────────────────────────────────────────────────────────────────

public sealed class EmailAlreadyExistsException : AppException
{
    public const string Code = "EMAIL_ALREADY_EXISTS";

    public string Email { get; }

    public EmailAlreadyExistsException(string email, string localizationKey = "EmailAlreadyExists",
        string resourceFile = "Common", object[]? args = null, string? devMessage = null)
        : base(Code, localizationKey, resourceFile, args ?? [email], devMessage)
    {
        Email = email;
    }
}