namespace FlowTask.Application.Exceptions;

/// <summary>Not authenticated / invalid or expired credentials (401-equivalent).</summary>
public sealed class UnauthenticatedException : AppException
{
    public const string Code = "UNAUTHENTICATED";

    public UnauthenticatedException(string localizationKey = "Unauthenticated", string resourceFile = "Common",
        object[]? args = null, string? devMessage = null)
        : base(Code, localizationKey, resourceFile, args, devMessage) { }
}