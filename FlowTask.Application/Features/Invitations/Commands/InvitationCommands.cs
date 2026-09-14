namespace FlowTask.Application.Features.Invitations.Commands;

using MediatR;
using FlowTask.Application.Features.Invitations.DTOs;

public record SendInvitationCommand(
    int    WorkspaceId,
    string Email,
    string Role = "member"
) : IRequest<InvitationDto>;

public record AcceptInvitationCommand(string Token) : IRequest<bool>;
public record DeclineInvitationCommand(string Token) : IRequest<bool>;
public record CancelInvitationCommand(int InvitationId) : IRequest<bool>;