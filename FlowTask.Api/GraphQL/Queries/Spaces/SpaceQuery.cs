namespace FlowTask.Api.GraphQL.Queries.Spaces;

using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using FlowTask.Application.Features.Spaces.DTOs;
using FlowTask.Application.Features.Spaces.Queries;

[ExtendObjectType("Query")]
public class SpaceQuery
{
    [Authorize]
    [GraphQLName("getSpacesByWorkspace")]
    public async Task<List<SpaceDto>> GetSpacesByWorkspace(
        int workspaceId,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetSpacesByWorkspaceQuery(workspaceId), ct);
}