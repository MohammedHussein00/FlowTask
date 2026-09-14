namespace FlowTask.Application.Features.Webhooks.Commands;

using MediatR;
using FlowTask.Application.Features.Webhooks.DTOs;

public record CreateWebhookCommand(
    int     WorkspaceId,
    string  Url,
    string? Events = null
) : IRequest<WebhookDto>;

public record DeleteWebhookCommand(int WebhookId) : IRequest<bool>;
public record ToggleWebhookCommand(int WebhookId) : IRequest<bool>;