namespace FlowTask.Application.Features.Workspaces.Commands.CreateWorkspace;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Workspaces.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Identity;

public class CreateWorkspaceCommandHandler
    : IRequestHandler<CreateWorkspaceCommand, WorkspaceDto>
{
    private readonly IApplicationDbContext  _db;
    private readonly ICurrentUserService    _currentUser;

    public CreateWorkspaceCommandHandler(
        IApplicationDbContext db,
        ICurrentUserService   currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<WorkspaceDto> Handle(CreateWorkspaceCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId
            ?? throw new UnauthorizedException();

        var workspace = new Workspace
        {
            Name      = request.Name,
            AvatarUrl = request.AvatarUrl,
            OwnerId   = userId
        };

        _db.Workspaces.Add(workspace);

        // Add owner as admin member
        var member = new WorkspaceMember
        {
            Workspace = workspace,
            UserId    = userId,
            Role      = "admin"
        };
        _db.WorkspaceMembers.Add(member);

        await _db.SaveChangesAsync(ct);

        var owner = await _db.Workspaces
            .Where(w => w.Id == workspace.Id)
            .Select(w => w.Owner)
            .FirstAsync(ct);

        return new WorkspaceDto(
            workspace.Id,
            workspace.Name,
            workspace.AvatarUrl,
            workspace.OwnerId,
            owner.FullName ?? owner.UserName!,
            1,
            workspace.CreatedAt);
    }
}