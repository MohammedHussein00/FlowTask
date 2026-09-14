namespace FlowTask.Application.Features.Auth.OAuth;

/// <summary>Normalised profile returned by any provider after a successful code exchange.</summary>
public sealed record OAuthUserInfo(
    string ProviderUserId,
    string Email,
    bool EmailVerified,
    string? FullName,
    string? AvatarUrl);