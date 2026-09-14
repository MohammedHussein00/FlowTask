namespace FlowTask.Shared.Results;

/// <summary>
/// Result<T> — mirrors FlowTask.Application.Exceptions.AppException field-for-field
/// (Code / LocalizationKey / ResourceFile / Args) so any handler can switch between
/// throwing and returning without changing how the error eventually gets localized.
/// Use Result<T> for expected business outcomes (not found, conflict, validation).
/// Keep throwing AppException for anything that should short-circuit deep call
/// stacks (e.g. a guard clause three layers down you don't want to thread through).
/// </summary>
public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public ResultError? Error { get; }

    private Result(T value) { IsSuccess = true; Value = value; }
    private Result(ResultError error) { IsSuccess = false; Error = error; }

    public static Result<T> Ok(T value) => new(value);
    public static Result<T> Fail(ResultError error) => new(error);

    // ── Convenience factories — one per AppException subtype ────────────
    public static Result<T> NotFound(string key, string file = "Common", params object[] args)
        => new(ResultError.Of(ErrorKind.NotFound, ErrorCodes.NotFound, key, file, args));

    public static Result<T> Conflict(string key, string file = "Common", params object[] args)
        => new(ResultError.Of(ErrorKind.Conflict, ErrorCodes.Conflict, key, file, args));

    public static Result<T> BadRequest(string key, string file = "Common", params object[] args)
        => new(ResultError.Of(ErrorKind.BadRequest, ErrorCodes.BadRequest, key, file, args));

    public static Result<T> Unauthorized(string key = "Unauthenticated", string file = "Common", params object[] args)
        => new(ResultError.Of(ErrorKind.Unauthorized, ErrorCodes.Unauthorized, key, file, args));

    public static Result<T> Forbidden(string key = "Forbidden", string file = "Common", params object[] args)
        => new(ResultError.Of(ErrorKind.Forbidden, ErrorCodes.Forbidden, key, file, args));

    public static Result<T> EmailExists(string email)
        => new(ResultError.Of(ErrorKind.Conflict, ErrorCodes.EmailAlreadyExists,
               "EmailAlreadyExists", "Auth", [email]));

    public static Result<T> Validation(
        IDictionary<string, IReadOnlyList<(string Key, object[] Args)>> fieldErrors)
        => new(ResultError.Validation(fieldErrors));
}

/// <summary>Non-generic Result for commands that return no payload (e.g. DeleteTask).</summary>
public sealed class Result
{
    public bool IsSuccess { get; }
    public ResultError? Error { get; }

    private Result(bool success, ResultError? error) { IsSuccess = success; Error = error; }

    public static Result Ok() => new(true, null);
    public static Result Fail(ResultError error) => new(false, error);
}

public enum ErrorKind
{
    Validation, BadRequest, Unauthorized, Forbidden, NotFound, Conflict, Unexpected
}

/// <summary>
/// Carries exactly what AppException carries, so one factory (LocalizedErrorFactory)
/// can build the client-facing ApiError from either a thrown exception or a
/// returned Result, using the SAME localization lookups.
/// </summary>
public sealed class ResultError
{
    public ErrorKind Kind { get; }
    public string Code { get; }
    public string LocalizationKey { get; }
    public string ResourceFile { get; }
    public object[] Args { get; }
    public IReadOnlyDictionary<string, IReadOnlyList<(string Key, object[] Args)>>? FieldErrors { get; }

    private ResultError(ErrorKind kind, string code, string key, string file, object[] args,
        IReadOnlyDictionary<string, IReadOnlyList<(string Key, object[] Args)>>? fieldErrors = null)
    {
        Kind = kind; Code = code; LocalizationKey = key; ResourceFile = file;
        Args = args; FieldErrors = fieldErrors;
    }

    public static ResultError Of(ErrorKind kind, string code, string key, string file, object[] args)
        => new(kind, code, key, file, args);

    public static ResultError Validation(
        IDictionary<string, IReadOnlyList<(string Key, object[] Args)>> fieldErrors)
    {
        // Surface the FIRST field failure's specific key/args as the top-level
        // LocalizationKey/Args, instead of the generic "ValidationFailed" —
        // so whatever ToPayload already does with LocalizationKey/ResourceFile/Args
        // shows the actual rule that failed (e.g. "PasswordRequiresSpecialChar")
        // without any change to ToPayload or ApiErrorType.
        //
        // Trade-off: if multiple fields fail at once, only the first one's
        // message surfaces at the top level. FieldErrors below still carries
        // every failure, so nothing is lost — it's just not all shown
        // simultaneously until ApiErrorType exposes a per-field list.
        var (key, args) = fieldErrors.Count > 0
            ? fieldErrors.First().Value.First()
            : ("ValidationFailed", Array.Empty<object>());

        return new(ErrorKind.Validation, ErrorCodes.ValidationError, key, "Common", args,
               new Dictionary<string, IReadOnlyList<(string, object[])>>(fieldErrors));
    }
}