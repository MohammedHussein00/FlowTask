namespace FlowTask.Application.Exceptions;

/// <summary>Resource already exists / state conflict (409-equivalent).</summary>
public class ConflictException : AppException
{
    public const string Code = "CONFLICT";

    public ConflictException(string localizationKey, string resourceFile = "Common",
        object[]? args = null, string? devMessage = null)
        : base(Code, localizationKey, resourceFile, args, devMessage) { }
}