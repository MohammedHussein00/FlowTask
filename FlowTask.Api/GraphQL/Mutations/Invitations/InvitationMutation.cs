namespace FlowTask.Api.GraphQL.Mutations.Invitations;

using FlowTask.Application.Features.Invitations.Commands;
using FlowTask.Application.Features.Invitations.DTOs;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Mutation")]
public class InvitationMutation
{
    [Authorize]
    public async Task<InvitationDto> SendInvitation(
        int workspaceId, string email, string role,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(
            new SendInvitationCommand(workspaceId, email, role), ct);

    [Authorize]
    public async Task<bool> AcceptInvitation(
        string token,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new AcceptInvitationCommand(token), ct);

    public async Task<bool> DeclineInvitation(
        string token,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeclineInvitationCommand(token), ct);

    [Authorize]
    public async Task<bool> CancelInvitation(
        int invitationId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CancelInvitationCommand(invitationId), ct);
}