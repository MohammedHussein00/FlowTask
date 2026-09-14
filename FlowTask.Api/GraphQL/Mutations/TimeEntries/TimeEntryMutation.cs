

namespace FlowTask.Api.GraphQL.Mutations.TimeEntries;

using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using FlowTask.Application.Features.TimeEntries.Commands;
using FlowTask.Application.Features.TimeEntries.DTOs;

[ExtendObjectType("Mutation")]
public class TimeEntryMutation
{
    [Authorize]
    public async Task<TimeEntryDto> LogTime(
        int taskId, DateTime startTime, DateTime endTime, string? description,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new LogTimeCommand(taskId, startTime, endTime, description), ct);

    [Authorize]
    public async Task<bool> DeleteTimeEntry(
        int id,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteTimeEntryCommand(id), ct);
}