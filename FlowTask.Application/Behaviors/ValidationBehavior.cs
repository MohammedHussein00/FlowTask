// FlowTask.Application/Behaviors/ValidationBehavior.cs
namespace FlowTask.Application.Behaviors;

using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using FlowTask.Shared.Results;

/// <summary>
/// MediatR pipeline behavior — runs all registered <see cref="IValidator{T}"/>s
/// for the incoming request. On failure, it does NOT throw — it short-circuits
/// the pipeline and returns a failed <c>Result</c>/<c>Result&lt;T&gt;</c> directly,
/// matching the pattern used everywhere else (EmailExists, NotFound, etc.).
/// This requires TResponse to be Result or Result&lt;T&gt; — both expose a
/// static Fail(ResultError) factory, located via reflection once per closed
/// generic type and cached.
///
/// The ResultError's FieldErrors dictionary expects, per field, a list of
/// (localizationKey, args) pairs — not raw English strings — so each failure's
/// FluentValidation ErrorCode becomes the localization key directly. Validators
/// set an explicit code via .WithErrorCode("SomeResxKey"); if a rule doesn't
/// set one, FluentValidation's default validator-type code (e.g.
/// "NotEmptyValidator") falls back to a generic bucket key via
/// <see cref="MapRuleCode"/>. FluentValidation's placeholder values (e.g. a
/// configured MinimumLength) become that template's Args.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    // Located once per closed generic TResponse (e.g. Result<AuthResultDto>,
    // Result) and cached — reflection cost is paid at most once per response type,
    // not per request.
    private static readonly MethodInfo? FailMethod =
        typeof(TResponse).GetMethod(
            "Fail",
            BindingFlags.Public | BindingFlags.Static,
            binder: null,
            types: [typeof(ResultError)],
            modifiers: null);

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, ct)));

        var fieldErrors = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .GroupBy(f => ToCamel(f.PropertyName))
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<(string Key, object[] Args)>)g
                    .Select(f => (MapRuleCode(f.ErrorCode), ExtractArgs(f)))
                    .ToList());

        if (fieldErrors.Count == 0)
            return await next();

        var error = ResultError.Validation(fieldErrors);

        if (FailMethod is null)
        {
            // TResponse isn't Result/Result<T> — there's no way to short-circuit
            // without throwing. This should only happen if ValidationBehavior is
            // ever wired up for a request whose response type doesn't follow the
            // Result pattern; surfacing it loudly is safer than swallowing it.
            throw new InvalidOperationException(
                $"{typeof(TResponse).Name} has no static Fail(ResultError) method — " +
                "ValidationBehavior only supports Result or Result<T> response types.");
        }

        return (TResponse)FailMethod.Invoke(null, [error])!;
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static string ToCamel(string s) =>
        string.IsNullOrEmpty(s) ? s : char.ToLowerInvariant(s[0]) + s[1..];

    private static object[] ExtractArgs(ValidationFailure f) =>
        f.FormattedMessagePlaceholderValues?
            .Where(kv => kv.Key is not ("PropertyName" or "PropertyValue"))
            .Select(kv => kv.Value)
            .ToArray()
        ?? Array.Empty<object>();

    private static string MapRuleCode(string fvCode) => fvCode switch
    {
        "NotEmptyValidator" => ErrorCodes.Required,
        "NotNullValidator" => ErrorCodes.Required,
        "MinimumLengthValidator" => ErrorCodes.MinLength,
        "ExactLengthValidator" => ErrorCodes.MinLength,
        "MaximumLengthValidator" => ErrorCodes.MaxLength,
        "LengthValidator" => ErrorCodes.MaxLength,
        "EmailValidator" => ErrorCodes.EmailFormat,
        "AspNetCoreCompatibleEmailValidator" => ErrorCodes.EmailFormat,
        "RegularExpressionValidator" => ErrorCodes.Pattern,
        _ when !fvCode.EndsWith("Validator", StringComparison.Ordinal) => fvCode,
        _ => ErrorCodes.ValidationError,
    };
}