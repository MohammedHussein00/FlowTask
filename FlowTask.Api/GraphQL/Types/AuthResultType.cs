namespace FlowTask.Api.GraphQL.Types;

using FlowTask.Application.Features.Auth.DTOs;

public class AuthResultType
{
    public int      UserId      { get; init; }
    public string   Email       { get; init; } = default!;
    public string   FullName    { get; init; } = default!;
    public string   AccessToken { get; init; } = default!;
    public DateTime ExpiresAt   { get; init; }

    // RefreshToken intentionally NOT exposed here — it's set as an httpOnly
    // cookie by AuthMutation and should never appear in a GraphQL response
    // body (network logs, devtools, error-monitoring payloads, etc.).
    public static AuthResultType FromDto(AuthResultDto dto) => new()
    {
        UserId      = dto.UserId,
        Email       = dto.Email,
        FullName    = dto.FullName,
        AccessToken = dto.AccessToken,
        ExpiresAt   = dto.ExpiresAt
    };
}