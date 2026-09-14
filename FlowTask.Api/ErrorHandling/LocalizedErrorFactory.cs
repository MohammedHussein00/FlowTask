namespace FlowTask.Api.ErrorHandling;

using FlowTask.Application.Exceptions;
using FlowTask.Shared.Localization;
using FlowTask.Shared.Results;

public static class LocalizedErrorFactory
{
    public static ApiError Build(AppException ex, ILocalizationService loc) =>
        BuildCore(ex.Code, ex.LocalizationKey, ex.ResourceFile, ex.Args,
                  ex is ValidationException v ? v.FieldErrors : null, loc);

    public static ApiError Build(ResultError err, ILocalizationService loc) =>
        BuildCore(err.Code, err.LocalizationKey, err.ResourceFile, err.Args, err.FieldErrors, loc);

    private static ApiError BuildCore(
        string code, string key, string resourceFile, object[] args,
        IReadOnlyDictionary<string, IReadOnlyList<(string Key, object[] Args)>>? fieldErrors,
        ILocalizationService loc)
    {
        if (fieldErrors is not null)
        {
            var errors = fieldErrors
                .SelectMany(kv => kv.Value.Select(e => new ApiFieldError(
                    field: kv.Key,
                    // Use the ResultError's ResourceFile, not hardcoded "Validation"
                    message: loc.Get(e.Key, resourceFile, e.Args),
                    code: e.Key)))
                .ToList();

            return ApiError.Validation(loc.Get(key, resourceFile), errors);
        }

        var message = loc.Get(key, resourceFile, args);
        return new ApiError { Code = code, Message = message, Errors = [] };
    }
}