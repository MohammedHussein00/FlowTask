namespace FlowTask.Api.GraphQL.Queries.Users;

using FlowTask.Application.Features.Users.DTOs;
using FlowTask.Application.Features.Users.Queries;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Query")]
public class UserQuery
{
    [Authorize]
    public async Task<UserProfileDto> Me(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetCurrentUserQuery(), ct);

    [Authorize]
    public async Task<List<UserDto>> GetWorkspaceMembers(
        int workspaceId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetWorkspaceMembersQuery(workspaceId), ct);
}