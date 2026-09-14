namespace FlowTask.Application.Features.Auth.OAuth;

/// <summary>
/// One implementation per provider (Google, GitHub). Register all implementations
/// in DI and resolve the right one via IEnumerable&lt;IOAuthProviderService&gt;.
/// </summary>
public interface IOAuthProviderService
{
    OAuthProvider Provider { get; }

    /// <summary>
    /// Exchanges the authorization code the frontend received from the popup
    /// for the user's profile. Throws <see cref="OAuthExchangeException"/> on
    /// any failure (invalid/expired code, provider outage, malformed response)
    /// — never returns a partially-populated result.
    /// </summary>
    Task<OAuthUserInfo> ExchangeCodeAsync(string code, CancellationToken ct);
}