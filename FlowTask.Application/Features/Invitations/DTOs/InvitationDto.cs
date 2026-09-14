namespace FlowTask.Application.Features.Invitations.DTOs;

public record InvitationDto(
    int      Id,
    int      WorkspaceId,
    string   WorkspaceName,
    string   Email,
    int      InvitedBy,
    string   InvitedByName,
    string   Role,
    string   Status,
    DateTime? ExpiresAt,
    DateTime CreatedAt
);