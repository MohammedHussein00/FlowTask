namespace FlowTask.Application.Exceptions;

/// <summary>
/// Field-level validation failure (400-equivalent in GraphQL terms).
///
/// Each entry's value is a list of (LocalizationKey, Args) pairs rather
/// than raw strings, so GraphQLErrorFilter can localize every individual
/// field message according to the request's language — not just the
/// top-level exception message.
/// </summary>
public class ValidationException : AppException
{
    public const string Code = "VALIDATION_ERROR";

    /// <summary>Field name → list of (resourceKey, args) to localize.</summary>
    public IReadOnlyDictionary<string, IReadOnlyList<(string Key, object[] Args)>> FieldErrors { get; }

    public ValidationException(
        IDictionary<string, IReadOnlyList<(string Key, object[] Args)>> fieldErrors,
        string resourceFile = "Validators")
        : base(
            Code,
            localizationKey: "ValidationFailed",
            resourceFile: resourceFile,
            args: null,
            devMessage: "One or more validation errors occurred.")
    {
        FieldErrors = new Dictionary<string, IReadOnlyList<(string, object[])>>(fieldErrors);
    }

    /// <summary>
    /// Convenience constructor for raw (field, rawEnglishMessage) pairs —
    /// used as a fallback when a message doesn't have a resource key yet
    /// (e.g. messages coming from a third-party library like ASP.NET
    /// Identity that hasn't been wrapped in LocalizedIdentityErrorDescriber).
    /// These are passed through as-is (already English) rather than looked
    /// up as resource keys.
    /// </summary>
    public static ValidationException FromRawMessages(IDictionary<string, string[]> rawErrors)
    {
        var mapped = rawErrors.ToDictionary(
            kv => kv.Key,
            kv => (IReadOnlyList<(string, object[])>)kv.Value
                .Select(m => (m, Array.Empty<object>()))
                .ToList());

        var ex = new ValidationException(mapped, resourceFile: "__raw__");
        return ex;
    }
}