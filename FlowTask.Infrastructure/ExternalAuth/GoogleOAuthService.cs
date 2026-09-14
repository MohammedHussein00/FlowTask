namespace FlowTask.Infrastructure.ExternalAuth;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using FlowTask.Application.Features.Auth.OAuth;

/// <summary>
/// Exchanges a Google authorization code (obtained via the frontend's
/// google.accounts.oauth2.initCodeClient popup) for the user's profile.
///
/// Registered as a typed HttpClient in Program.cs:
///   builder.Services.AddHttpClient&lt;IOAuthProviderService, GoogleOAuthService&gt;();
/// which is why the first constructor parameter must be HttpClient.
/// </summary>
public class GoogleOAuthService : IOAuthProviderService
{
    private readonly HttpClient _httpClient;
    private readonly GoogleOAuthOptions _options;

    public GoogleOAuthService(HttpClient httpClient, IOptions<GoogleOAuthOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public OAuthProvider Provider => OAuthProvider.Google;

    public async Task<OAuthUserInfo> ExchangeCodeAsync(string code, CancellationToken ct)
    {
        var tokenResponse = await ExchangeCodeForTokenAsync(code, ct);
        var profile = await FetchProfileAsync(tokenResponse.AccessToken, ct);

        if (string.IsNullOrWhiteSpace(profile.Email))
            throw new OAuthExchangeException("email_not_verified", "Google account has no email on file.");

        return new OAuthUserInfo(
            ProviderUserId: profile.Sub,
            Email: profile.Email,
            EmailVerified: profile.EmailVerified,
            FullName: profile.Name,
            AvatarUrl: profile.Picture);
    }

    private async Task<TokenResponse> ExchangeCodeForTokenAsync(string code, CancellationToken ct)
    {
        try
        {
            var form = new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret,
                ["redirect_uri"] = _options.RedirectUri,
                ["grant_type"] = "authorization_code",
            };

            using var response = await _httpClient.PostAsync(
                "https://oauth2.googleapis.com/token", new FormUrlEncodedContent(form), ct);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                throw new OAuthExchangeException("invalid_grant",
                    $"Google token exchange failed ({(int)response.StatusCode}): {body}");
            }

            return await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: ct)
                ?? throw new OAuthExchangeException("invalid_response", "Google returned an empty token response.");
        }
        catch (OAuthExchangeException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new OAuthExchangeException("provider_unreachable", "Could not reach Google's token endpoint.", ex);
        }
    }

    private async Task<GoogleUserInfoResponse> FetchProfileAsync(string accessToken, CancellationToken ct)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v3/userinfo");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var response = await _httpClient.SendAsync(request, ct);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                throw new OAuthExchangeException("profile_fetch_failed",
                    $"Google profile fetch failed ({(int)response.StatusCode}): {body}");
            }

            return await response.Content.ReadFromJsonAsync<GoogleUserInfoResponse>(cancellationToken: ct)
                ?? throw new OAuthExchangeException("invalid_response", "Google returned an empty profile response.");
        }
        catch (OAuthExchangeException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new OAuthExchangeException("provider_unreachable", "Could not reach Google's userinfo endpoint.", ex);
        }
    }

    private sealed class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = default!;
    }

    private sealed class GoogleUserInfoResponse
    {
        [JsonPropertyName("sub")]
        public string Sub { get; set; } = default!;

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("picture")]
        public string? Picture { get; set; }
    }
}