namespace FlowTask.Application.Exceptions;

/// <summary>Requested resource doesn't exist (404-equivalent).</summary>
public class NotFoundException : AppException
{
    public const string Code = "NOT_FOUND";

    public NotFoundException(string localizationKey, string resourceFile = "Common",
        object[]? args = null, string? devMessage = null)
        : base(Code, localizationKey, resourceFile, args, devMessage) { }
}