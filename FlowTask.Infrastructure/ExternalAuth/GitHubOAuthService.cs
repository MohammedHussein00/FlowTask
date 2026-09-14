namespace FlowTask.Infrastructure.ExternalAuth;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using FlowTask.Application.Features.Auth.OAuth;

/// <summary>
/// Exchanges a GitHub authorization code (captured from the popup opened in
/// register.component.ts) for the user's profile and primary verified email.
///
/// Registered as a typed HttpClient in Program.cs:
///   builder.Services.AddHttpClient&lt;IOAuthProviderService, GitHubOAuthService&gt;();
/// </summary>
public class GitHubOAuthService : IOAuthProviderService
{
    private readonly HttpClient _httpClient;
    private readonly GitHubOAuthOptions _options;

    public GitHubOAuthService(HttpClient httpClient, IOptions<GitHubOAuthOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;

        // Required by GitHub's API for every request, or you get a 403.
        if (!_httpClient.DefaultRequestHeaders.UserAgent.Any())
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("FlowTask");
    }

    public OAuthProvider Provider => OAuthProvider.GitHub;

    public async Task<OAuthUserInfo> ExchangeCodeAsync(string code, CancellationToken ct)
    {
        var accessToken = await ExchangeCodeForTokenAsync(code, ct);

        var profile = await GetAsync<GitHubUserResponse>("https://api.github.com/user", accessToken, ct)
            ?? throw new OAuthExchangeException("invalid_response", "GitHub returned an empty profile response.");

        var emails = await GetAsync<List<GitHubEmailResponse>>("https://api.github.com/user/emails", accessToken, ct)
            ?? new List<GitHubEmailResponse>();

        var primaryEmail = emails.FirstOrDefault(e => e.Primary && e.Verified) ?? emails.FirstOrDefault(e => e.Verified);
        var email = primaryEmail?.Email ?? profile.Email;

        if (string.IsNullOrWhiteSpace(email))
            throw new OAuthExchangeException("email_not_verified",
                "No verified email is available on this GitHub account. Make an email public or verified on GitHub and try again.");

        return new OAuthUserInfo(
            ProviderUserId: profile.Id.ToString(),
            Email: email,
            EmailVerified: primaryEmail?.Verified ?? false,
            FullName: profile.Name ?? profile.Login,
            AvatarUrl: profile.AvatarUrl);
    }

    private async Task<string> ExchangeCodeForTokenAsync(string code, CancellationToken ct)
    {
        try
        {
            var form = new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
                ["redirect_uri"] = _options.RedirectUri,
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token")
            {
                Content = new FormUrlEncodedContent(form),
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(request, ct);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                throw new OAuthExchangeException("invalid_grant",
                    $"GitHub token exchange failed ({(int)response.StatusCode}): {body}");
            }

            var token = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: ct)
                ?? throw new OAuthExchangeException("invalid_response", "GitHub returned an empty token response.");

            if (!string.IsNullOrWhiteSpace(token.Error))
                throw new OAuthExchangeException(token.Error!,
                    token.ErrorDescription ?? "GitHub rejected the authorization code.");

            return token.AccessToken;
        }
        catch (OAuthExchangeException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new OAuthExchangeException("provider_unreachable", "Could not reach GitHub's token endpoint.", ex);
        }
    }

    private async Task<T?> GetAsync<T>(string url, string accessToken, CancellationToken ct)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));

            using var response = await _httpClient.SendAsync(request, ct);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                throw new OAuthExchangeException("profile_fetch_failed",
                    $"GitHub request to {url} failed ({(int)response.StatusCode}): {body}");
            }

            return await response.Content.ReadFromJsonAsync<T>(cancellationToken: ct);
        }
        catch (OAuthExchangeException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new OAuthExchangeException("provider_unreachable", $"Could not reach GitHub ({url}).", ex);
        }
    }

    private sealed class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("error_description")]
        public string? ErrorDescription { get; set; }
    }

    private sealed class GitHubUserResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("login")]
        public string Login { get; set; } = default!;

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }
    }

    private sealed class GitHubEmailResponse
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = default!;

        [JsonPropertyName("primary")]
        public bool Primary { get; set; }

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }
    }
}