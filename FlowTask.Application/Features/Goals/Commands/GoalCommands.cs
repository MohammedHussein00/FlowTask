namespace FlowTask.Application.Features.Goals.Commands;

using MediatR;
using FlowTask.Application.Features.Goals.DTOs;

public record CreateGoalCommand(
    int      WorkspaceId,
    string   Name,
    string?  Description = null,
    DateTime? DueDate    = null,
    string   Color       = "#3498DB"
) : IRequest<GoalDto>;

public record UpdateKeyResultProgressCommand(
    int     KeyResultId,
    decimal CurrentValue
) : IRequest<KeyResultDto>;

public record AddKeyResultCommand(
    int      GoalId,
    string   Name,
    string   ResultType   = "numeric",
    decimal? TargetValue  = null
) : IRequest<KeyResultDto>;