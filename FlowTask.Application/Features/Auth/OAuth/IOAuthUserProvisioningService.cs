namespace FlowTask.Application.Features.Auth.OAuth;

using FlowTask.Application.Features.Auth.DTOs;

/// <summary>
/// The one piece of this feature that touches your actual user store. Everything
/// else in this OAuth flow (Google/GitHub exchange, GraphQL surface, error mapping)
/// is provider-agnostic and doesn't need to know how users/tokens are persisted.
///
/// Implement this in Infrastructure using whatever RegisterCommandHandler /
/// LoginCommandHandler already use (UserManager&lt;T&gt;, your own repository,
/// IJwtTokenService, etc.) — the contract below is deliberately minimal.
/// </summary>
public interface IOAuthUserProvisioningService
{
    /// <summary>
    /// Finds an existing user by email (or by a previously-linked external
    /// login for this provider+providerUserId, if you store that), creating
    /// one if none exists, then issues access/refresh tokens exactly like a
    /// normal login. Should mark the account email as verified when
    /// info.EmailVerified is true, since the provider already vouched for it.
    /// </summary>
    Task<AuthResultDto> GetOrCreateAndSignInAsync(
        OAuthProvider provider, OAuthUserInfo info, CancellationToken ct);
}