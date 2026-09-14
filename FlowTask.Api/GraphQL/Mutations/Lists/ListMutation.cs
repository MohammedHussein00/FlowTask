namespace FlowTask.Api.GraphQL.Mutations.Lists;

using FlowTask.Application.Features.Lists.Commands.CreateList;
using FlowTask.Application.Features.Lists.Commands.UpdateList;
using FlowTask.Application.Features.Lists.DTOs;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Mutation")]
public class ListMutation
{
    [Authorize]
    public async Task<ListDto> CreateList(
        int spaceId, string name, int? folderId,
        string? content, string priority,
        DateTime? startDate, DateTime? dueDate,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateListCommand(
            spaceId, name, folderId, content, priority, startDate, dueDate), ct);

    [Authorize]
    public async Task<ListDto> UpdateList(
        int id, string name, string? content,
        string priority, DateTime? startDate, DateTime? dueDate,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new UpdateListCommand(
            id, name, content, priority, startDate, dueDate), ct);
}