namespace FlowTask.Api.GraphQL.Mutations.Webhooks;

using FlowTask.Application.Features.Webhooks.Commands;
using FlowTask.Application.Features.Webhooks.DTOs;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Mutation")]
public class WebhookMutation
{
    [Authorize]
    public async Task<WebhookDto> CreateWebhook(
        int workspaceId, string url, string? events,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(
            new CreateWebhookCommand(workspaceId, url, events), ct);

    [Authorize]
    public async Task<bool> DeleteWebhook(
        int webhookId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteWebhookCommand(webhookId), ct);

    [Authorize]
    public async Task<bool> ToggleWebhook(
        int webhookId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ToggleWebhookCommand(webhookId), ct);
}