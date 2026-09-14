namespace FlowTask.Application.Features.Auth.DTOs;

public record AuthResultDto(
    int    UserId,
    string Email,
    string FullName,
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt
);