namespace FlowTask.Application.Features.Auth.OAuth;

/// <summary>
/// Raised by IOAuthProviderService implementations when the code exchange or
/// profile fetch fails for any reason. ErrorCode is a short machine-readable
/// tag (e.g. "invalid_grant", "email_not_verified", "provider_unreachable")
/// used to pick the right localization key / log detail one layer up.
/// </summary>
public sealed class OAuthExchangeException : Exception
{
    public string ErrorCode { get; }

    public OAuthExchangeException(string errorCode, string message, Exception? inner = null)
        : base(message, inner)
        => ErrorCode = errorCode;
}