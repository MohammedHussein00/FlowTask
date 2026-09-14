namespace FlowTask.Application.Exceptions;

/// <summary>Authenticated, but not allowed to do this (403-equivalent).</summary>
public class ForbiddenException : AppException
{
    public const string Code = "FORBIDDEN";

    public ForbiddenException(string localizationKey = "Forbidden", string resourceFile = "Common",
        object[]? args = null, string? devMessage = null)
        : base(Code, localizationKey, resourceFile, args, devMessage) { }
}