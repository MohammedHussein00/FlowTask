namespace FlowTask.Application.Features.Workspaces.Queries.GetWorkspace;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Workspaces.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Identity;

public class GetWorkspaceByIdQueryHandler
    : IRequestHandler<GetWorkspaceByIdQuery, WorkspaceDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public GetWorkspaceByIdQueryHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<WorkspaceDto> Handle(GetWorkspaceByIdQuery request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var workspace = await _db.Workspaces
            .Include(w => w.Owner)
            .Include(w => w.Members)
            .FirstOrDefaultAsync(w =>
                w.Id == request.Id &&
                w.Members.Any(m => m.UserId == userId), ct)
            ?? throw new NotFoundException(localizationKey: "WorkspaceNotFound", args: [request.Id], devMessage: $"Workspace with id '{request.Id}' was not found.");

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