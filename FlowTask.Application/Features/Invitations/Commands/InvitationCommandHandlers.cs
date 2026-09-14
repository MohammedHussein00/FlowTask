namespace FlowTask.Application.Features.Invitations.Commands;

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Invitations.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Identity;
using FlowTask.Domain.Security;

public class SendInvitationCommandHandler
    : IRequestHandler<SendInvitationCommand, InvitationDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public SendInvitationCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<InvitationDto> Handle(
        SendInvitationCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var workspace = await _db.Workspaces
            .Include(w => w.Members)
            .FirstOrDefaultAsync(w => w.Id == request.WorkspaceId, ct)
            ?? throw new NotFoundException(
                localizationKey: "WorkspaceNotFound",
                args: [request.WorkspaceId],
                devMessage: $"Workspace with id '{request.WorkspaceId}' was not found.");

        var isAdmin = workspace.Members
            .Any(m => m.UserId == userId && m.Role == "admin");
        if (!isAdmin)
            throw new UnauthorizedException(
                localizationKey: "OnlyAdminsCanSendInvitations",
                devMessage: "Only workspace admins can send invitations.");

        // Check if already a member
        var inviter = workspace.Members.FirstOrDefault(m => m.UserId == userId);

        // Check for existing pending invitation
        var existingInvite = await _db.Invitations
            .AnyAsync(i =>
                i.WorkspaceId == request.WorkspaceId &&
                i.Email == request.Email &&
                i.Status == "pending", ct);

        if (existingInvite)
            throw new ConflictException(
                localizationKey: "PendingInvitationAlreadyExists",
                args: [request.Email],
                devMessage: $"A pending invitation already exists for {request.Email}.");

        var token = Guid.NewGuid().ToString("N");
        var invitation = new Invitation
        {
            WorkspaceId = request.WorkspaceId,
            Email = request.Email,
            InvitedBy = userId,
            Role = request.Role,
            Status = "pending",
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _db.Invitations.Add(invitation);
        await _db.SaveChangesAsync(ct);

        var inviter2 = await _db.Workspaces
            .Where(w => w.Id == request.WorkspaceId)
            .Select(w => w.Owner)
            .FirstAsync(ct);

        return new InvitationDto(
            invitation.Id, invitation.WorkspaceId,
            workspace.Name, invitation.Email,
            invitation.InvitedBy,
            inviter2.FullName ?? inviter2.UserName!,
            invitation.Role, invitation.Status,
            invitation.ExpiresAt, invitation.CreatedAt);
    }
}

public class AcceptInvitationCommandHandler
    : IRequestHandler<AcceptInvitationCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public AcceptInvitationCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(AcceptInvitationCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();
        var invitation = await _db.Invitations
            .FirstOrDefaultAsync(i =>
                i.Token == request.Token &&
                i.Status == "pending", ct)
            ?? throw new NotFoundException(
                localizationKey: "InvitationNotFound",
                args: [request.Token],
                devMessage: $"Invitation with token '{request.Token}' was not found.");

        if (invitation.ExpiresAt < DateTime.UtcNow)
            throw ValidationException.FromRawMessages(
                new Dictionary<string, string[]>
                {
                    ["Token"] = ["This invitation has expired."]
                });

        invitation.Status = "accepted";

        // Add user as workspace member
        var alreadyMember = await _db.WorkspaceMembers
            .AnyAsync(m => m.WorkspaceId == invitation.WorkspaceId && m.UserId == userId, ct);

        if (!alreadyMember)
        {
            _db.WorkspaceMembers.Add(new WorkspaceMember
            {
                WorkspaceId = invitation.WorkspaceId,
                UserId = userId,
                Role = invitation.Role
            });
        }

        await _db.SaveChangesAsync(ct);
        return true;
    }
}

public class DeclineInvitationCommandHandler
    : IRequestHandler<DeclineInvitationCommand, bool>
{
    private readonly IApplicationDbContext _db;

    public DeclineInvitationCommandHandler(IApplicationDbContext db) => _db = db;

    public async Task<bool> Handle(DeclineInvitationCommand request, CancellationToken ct)
    {
        var invitation = await _db.Invitations
            .FirstOrDefaultAsync(i =>
                i.Token == request.Token && i.Status == "pending", ct)
            ?? throw new NotFoundException(
                localizationKey: "InvitationNotFound",
                args: [request.Token],
                devMessage: $"Invitation with token '{request.Token}' was not found.");

        invitation.Status = "declined";
        await _db.SaveChangesAsync(ct);
        return true;
    }
}

public class CancelInvitationCommandHandler
    : IRequestHandler<CancelInvitationCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CancelInvitationCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(CancelInvitationCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();
        var invitation = await _db.Invitations
            .FirstOrDefaultAsync(i => i.Id == request.InvitationId, ct)
            ?? throw new NotFoundException(
                localizationKey: "InvitationNotFound",
                args: [request.InvitationId],
                devMessage: $"Invitation with id '{request.InvitationId}' was not found.");

        if (invitation.InvitedBy != userId)
            throw new UnauthorizedException(
                localizationKey: "OnlySenderCanCancelInvitation",
                devMessage: "Only the sender can cancel this invitation.");

        invitation.Status = "cancelled";
        await _db.SaveChangesAsync(ct);
        return true;
    }
}