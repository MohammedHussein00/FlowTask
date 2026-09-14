namespace FlowTask.Application.Features.Users.DTOs;

public record UserDto(
    int     Id,
    string  Email,
    string  FullName,
    string? AvatarUrl,
    string  Timezone,
    List<string> Roles,
    DateTime CreatedAt
);

public record UserProfileDto(
    int     Id,
    string  Email,
    string  FullName,
    string? AvatarUrl,
    string  Timezone,
    bool    TwoFactorEnabled,
    string  UserName
);