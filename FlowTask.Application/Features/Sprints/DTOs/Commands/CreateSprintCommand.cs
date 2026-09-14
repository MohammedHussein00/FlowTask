namespace FlowTask.Application.Features.Sprints.Commands;

using MediatR;
using FlowTask.Application.Features.Sprints.DTOs;

public record CreateSprintCommand(
    int      ListId,
    string   Name,
    DateTime StartDate,
    DateTime EndDate,
    string?  Description = null,
    string?  Goal        = null
) : IRequest<SprintDto>;

public record UpdateSprintStatusCommand(int SprintId, string Status) : IRequest<SprintDto>;
public record AddTaskToSprintCommand(int SprintId, int TaskId, int? StoryPoints = null) : IRequest<bool>;
public record RemoveTaskFromSprintCommand(int SprintId, int TaskId) : IRequest<bool>;