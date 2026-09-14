namespace FlowTask.Application.Features.Webhooks.DTOs;

public record WebhookDto(
    int    Id,
    int    WorkspaceId,
    string Url,
    string? Events,
    bool   IsActive,
    DateTime CreatedAt
);