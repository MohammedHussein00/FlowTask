namespace FlowTask.Api.GraphQL.Mutations.Automations;

using FlowTask.Application.Features.Automations.Commands;
using FlowTask.Application.Features.Automations.DTOs;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Mutation")]
public class AutomationMutation
{
    [Authorize]
    public async Task<AutomationDto> CreateAutomation(
        int workspaceId, string name, string triggerType,
        string? description, int? spaceId, int? listId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateAutomationCommand(
            workspaceId, name, triggerType, description, spaceId, listId), ct);

    [Authorize]
    public async Task<bool> ToggleAutomation(
        int automationId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ToggleAutomationCommand(automationId), ct);

    [Authorize]
    public async Task<TriggerDto> AddAutomationTrigger(
        int automationId, string triggerEvent,
        string entityType, string? conditions,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new AddAutomationTriggerCommand(
            automationId, triggerEvent, entityType, conditions), ct);

    [Authorize]
    public async Task<ActionDto> AddAutomationAction(
        int automationId, string actionType,
        string? actionConfig, int? orderIndex,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new AddAutomationActionCommand(
            automationId, actionType, actionConfig, orderIndex), ct);

    [Authorize]
    public async Task<bool> DeleteAutomation(
        int automationId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteAutomationCommand(automationId), ct);
}