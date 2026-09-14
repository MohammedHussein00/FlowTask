namespace FlowTask.Application.Features.Workspaces.Commands.UpdateWorkspace;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Workspaces.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Identity;

public class UpdateWorkspaceCommandHandler
    : IRequestHandler<UpdateWorkspaceCommand, WorkspaceDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public UpdateWorkspaceCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<WorkspaceDto> Handle(UpdateWorkspaceCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var workspace = await _db.Workspaces
            .Include(w => w.Owner)
            .Include(w => w.Members)
            .FirstOrDefaultAsync(w => w.Id == request.Id, ct)
            ?? throw new
                     NotFoundException(localizationKey: "WorkspaceNotFound", args: [request.Id], devMessage: $"Workspace with id '{request.Id}' was not found.")
            ;

        if (workspace.OwnerId != userId)
            throw new UnauthorizedException("Only the workspace owner can update it.");

        workspace.Name      = request.Name;
        workspace.AvatarUrl = request.AvatarUrl;

        await _db.SaveChangesAsync(ct);

        return new WorkspaceDto(
            workspace.Id,
            workspace.Name,
            workspace.AvatarUrl,
            workspace.OwnerId,
            workspace.Owner.FullName ?? workspace.Owner.UserName!,
            workspace.Members.Count,
            workspace.CreatedAt);
    }
}