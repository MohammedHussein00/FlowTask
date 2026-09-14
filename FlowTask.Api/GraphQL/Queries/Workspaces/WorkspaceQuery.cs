namespace FlowTask.Api.GraphQL.Queries.Workspaces;

using System.Linq;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using FlowTask.Application.Features.Workspaces.Queries.GetWorkspace;
using FlowTask.Api.GraphQL.Types.Workspaces;
using FlowTask.Application.Features.Workspaces.Queries.GetMyWorkspaces;

[ExtendObjectType("Query")]
public class WorkspaceQuery
{
    [Authorize]
    public async Task<WorkspaceType> GetWorkspace(
        int id,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetWorkspaceByIdQuery(id), ct);
        return WorkspaceType.FromDto(result);
    }

    /// <summary>All workspaces the current user belongs to — powers the top-left workspace switcher.</summary>
    [Authorize]
    public async Task<List<WorkspaceType>> MyWorkspaces(
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetMyWorkspacesQuery(), ct);
        return result.Select(WorkspaceType.FromDto).ToList();
    }
}
