namespace FlowTask.Api.GraphQL.Mutations.Tasks;

using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using FlowTask.Application.Features.Tasks.Commands.AssignTask;
using FlowTask.Application.Features.Tasks.Commands.CreateTask;
using FlowTask.Application.Features.Tasks.Commands.DeleteTask;
using FlowTask.Application.Features.Tasks.Commands.UpdateTask;
using FlowTask.Api.GraphQL.Inputs.Tasks;
using FlowTask.Api.GraphQL.Types.Tasks;

[ExtendObjectType("Mutation")]
public class TaskMutation
{
    [Authorize]
    public async Task<TaskType> CreateTask(
        CreateTaskInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(new CreateTaskCommand(
            input.ListId, input.Name, input.Description,
            input.Priority, input.StatusId, input.ParentId,
            input.StartDate, input.DueDate, input.TimeEstimate,
            input.AssigneeIds), ct);
        return TaskType.FromDto(result);
    }

    [Authorize]
    public async Task<TaskType> UpdateTask(
        UpdateTaskInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateTaskCommand(
            input.Id, input.Name, input.Description,
            input.Priority, input.StatusId,
            input.StartDate, input.DueDate, input.TimeEstimate), ct);
        return TaskType.FromDto(result);
    }

    [Authorize]
    public async Task<bool> DeleteTask(
        int id,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new DeleteTaskCommand(id), ct);

    [Authorize]
    public async Task<bool> AssignTask(
        AssignTaskInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new AssignTaskCommand(input.TaskId, input.UserId), ct);

    [Authorize]
    public async Task<bool> UnassignTask(
        AssignTaskInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new UnassignTaskCommand(input.TaskId, input.UserId), ct);
}