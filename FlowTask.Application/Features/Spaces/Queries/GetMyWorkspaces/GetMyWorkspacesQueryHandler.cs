namespace FlowTask.Application.Features.Workspaces.Queries.GetMyWorkspaces;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Workspaces.DTOs;
using FlowTask.Application.Interfaces;

public class GetMyWorkspacesQueryHandler
    : IRequestHandler<GetMyWorkspacesQuery, List<WorkspaceDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public GetMyWorkspacesQueryHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<WorkspaceDto>> Handle(GetMyWorkspacesQuery request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var workspaces = await _db.Workspaces
            .Include(w => w.Owner)
            .Include(w => w.Members)
            .Where(w => w.Members.Any(m => m.UserId == userId))
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync(ct);

        return workspaces
            .Select(w => new WorkspaceDto(
                w.Id,
                w.Name,
                w.AvatarUrl,
                w.OwnerId,
                w.Owner.FullName ?? w.Owner.UserName!,
                w.Members.Count,
                w.CreatedAt))
            .ToList();
    }
}