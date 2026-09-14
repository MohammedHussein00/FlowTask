namespace FlowTask.Application.Features.Workspaces.DTOs;

public record WorkspaceDto(
    int    Id,
    string Name,
    string? AvatarUrl,
    int    OwnerId,
    string OwnerName,
    int    MemberCount,
    DateTime CreatedAt
);