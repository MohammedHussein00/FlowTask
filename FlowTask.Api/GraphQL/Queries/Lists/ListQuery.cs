namespace FlowTask.Api.GraphQL.Queries.Lists;

using FlowTask.Application.Features.Lists.DTOs;
using FlowTask.Application.Features.Lists.Queries;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Query")]
public class ListQuery
{
    [Authorize]
    public async Task<List<ListDto>> GetListsBySpace(
        int spaceId,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetListsBySpaceQuery(spaceId), ct);
}