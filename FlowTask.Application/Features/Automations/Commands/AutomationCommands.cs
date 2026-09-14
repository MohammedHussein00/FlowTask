namespace FlowTask.Application.Features.Automations.Commands;

using MediatR;
using FlowTask.Application.Features.Automations.DTOs;

public record CreateAutomationCommand(
    int     WorkspaceId,
    string  Name,
    string  TriggerType,
    string? Description    = null,
    int?    SpaceId        = null,
    int?    ListId         = null,
    string? ConditionLogic = null
) : IRequest<AutomationDto>;

public record ToggleAutomationCommand(int AutomationId) : IRequest<bool>;

public record AddAutomationTriggerCommand(
    int     AutomationId,
    string  TriggerEvent,
    string  EntityType,
    string? Conditions = null
) : IRequest<TriggerDto>;

public record AddAutomationActionCommand(
    int     AutomationId,
    string  ActionType,
    string? ActionConfig = null,
    int?    OrderIndex   = null
) : IRequest<ActionDto>;

public record DeleteAutomationCommand(int AutomationId) : IRequest<bool>;