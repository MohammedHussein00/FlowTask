namespace FlowTask.Application.Features.Spaces.DTOs;

public record SpaceDto(
    int      Id,
    int      WorkspaceId,
    string   Name,
    bool     IsPrivate,
    string   Color,
    string?  AvatarUrl,
    int      MemberCount,
    DateTime CreatedAt
);