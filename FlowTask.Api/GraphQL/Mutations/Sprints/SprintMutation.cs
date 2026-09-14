namespace FlowTask.Api.GraphQL.Mutations.Sprints;

using FlowTask.Application.Features.Sprints.Commands;
using FlowTask.Application.Features.Sprints.DTOs;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Mutation")]
public class SprintMutation
{
    [Authorize]
    public async Task<SprintDto> CreateSprint(
        int listId, string name, DateTime startDate, DateTime endDate,
        string? description, string? goal,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateSprintCommand(
            listId, name, startDate, endDate, description, goal), ct);

    [Authorize]
    public async Task<SprintDto> UpdateSprintStatus(
        int sprintId, string status,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new UpdateSprintStatusCommand(sprintId, status), ct);

    [Authorize]
    public async Task<bool> AddTaskToSprint(
        int sprintId, int taskId, int? storyPoints,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new AddTaskToSprintCommand(sprintId, taskId, storyPoints), ct);

    [Authorize]
    public async Task<bool> RemoveTaskFromSprint(
        int sprintId, int taskId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new RemoveTaskFromSprintCommand(sprintId, taskId), ct);
}