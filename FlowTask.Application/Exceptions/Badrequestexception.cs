namespace FlowTask.Application.Exceptions;

/// <summary>Malformed / semantically invalid request that isn't a field-level
/// validation failure (generic 400-equivalent — e.g. "input state expired").</summary>
public sealed class BadRequestException : AppException
{
    public const string Code = "BAD_REQUEST";

    public BadRequestException(string localizationKey, string resourceFile = "Common",
        object[]? args = null, string? devMessage = null)
        : base(Code, localizationKey, resourceFile, args, devMessage) { }
}