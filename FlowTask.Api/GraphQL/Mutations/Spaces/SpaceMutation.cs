namespace FlowTask.Api.GraphQL.Mutations.Spaces;

using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using FlowTask.Api.GraphQL.Inputs.Spaces;
using FlowTask.Application.Features.Spaces.Commands.CreateSpace;
using FlowTask.Application.Features.Spaces.Commands.UpdateSpace;
using FlowTask.Application.Features.Spaces.DTOs;

[ExtendObjectType("Mutation")]
public class SpaceMutation
{
    [Authorize]
    [GraphQLName("createSpace")]
    public async Task<SpaceDto> CreateSpace(
        CreateSpaceInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new CreateSpaceCommand(
            input.WorkspaceId,
            input.Name,
            input.IsPrivate,
            input.Color,
            input.AvatarUrl), ct);

    [Authorize]
    [GraphQLName("updateSpace")]
    public async Task<SpaceDto> UpdateSpace(
        UpdateSpaceInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new UpdateSpaceCommand(
            input.Id,
            input.Name,
            input.IsPrivate,
            input.Color,
            input.AvatarUrl), ct);
}