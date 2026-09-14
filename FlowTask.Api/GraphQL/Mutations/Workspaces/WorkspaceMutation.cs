namespace FlowTask.Api.GraphQL.Mutations.Workspaces;

using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using FlowTask.Application.Features.Workspaces.Commands.CreateWorkspace;
using FlowTask.Application.Features.Workspaces.Commands.UpdateWorkspace;
using FlowTask.Api.GraphQL.Inputs.Workspaces;
using FlowTask.Api.GraphQL.Types.Workspaces;

[ExtendObjectType("Mutation")]
public class WorkspaceMutation
{
    [Authorize]
    public async Task<WorkspaceType> CreateWorkspace(
        CreateWorkspaceInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new CreateWorkspaceCommand(input.Name, input.AvatarUrl), ct);
        return WorkspaceType.FromDto(result);
    }

    [Authorize]
    public async Task<WorkspaceType> UpdateWorkspace(
        UpdateWorkspaceInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new UpdateWorkspaceCommand(input.Id, input.Name, input.AvatarUrl), ct);
        return WorkspaceType.FromDto(result);
    }
}