namespace FlowTask.Infrastructure.ExternalAuth;

/// <summary>Bound from configuration section "OAuth:Google". ClientSecret should
/// come from user-secrets / environment variables / a secret manager, never
/// committed to appsettings.json.</summary>
public class GoogleOAuthOptions
{
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;

    // Must match the redirect_uri your GIS code client actually used. With
    // ux_mode: 'popup' and no redirect_uri set on the frontend, Google Identity
    // Services uses the literal string "postmessage" — not a real URL.
    public string RedirectUri { get; set; } = "postmessage";
}

/// <summary>Bound from configuration section "OAuth:GitHub".</summary>
public class GitHubOAuthOptions
{
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;

    // Must exactly match the redirect_uri the popup was opened with in
    // register.component.ts (`${window.location.origin}/auth/register`).
    public string RedirectUri { get; set; } = default!;
}