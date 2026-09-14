namespace FlowTask.Api.GraphQL.Queries.Tasks;

using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using FlowTask.Api.GraphQL.Types.Tasks;
using FlowTask.Application.Features.Tasks.Queries.GetTaskById;
using FlowTask.Application.Features.Tasks.Queries.GetTasksByList;

[ExtendObjectType("Query")]
public class TaskQuery
{
    [Authorize]
    public async Task<TaskType> GetTask(
        int id,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetTaskByIdQuery(id), ct);
        return TaskType.FromDto(result);
    }

    [Authorize]
    public async Task<List<TaskType>> GetTasksByList(
        int listId,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var results = await mediator.Send(new GetTasksByListQuery(listId), ct);
        return results.Select(TaskType.FromDto).ToList();
    }
}