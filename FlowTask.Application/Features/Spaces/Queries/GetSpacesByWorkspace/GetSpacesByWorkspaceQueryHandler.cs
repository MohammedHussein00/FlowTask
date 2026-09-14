namespace FlowTask.Application.Features.Spaces.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Spaces.DTOs;
using FlowTask.Application.Interfaces;

public class GetSpacesByWorkspaceQueryHandler
    : IRequestHandler<GetSpacesByWorkspaceQuery, List<SpaceDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public GetSpacesByWorkspaceQueryHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<SpaceDto>> Handle(GetSpacesByWorkspaceQuery request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        return await _db.Spaces
            .Where(s => s.WorkspaceId == request.WorkspaceId &&
                        (!s.IsPrivate || s.Members.Any(m => m.UserId == userId)))
            .OrderBy(s => s.Name)
            .Select(s => new SpaceDto(
                s.Id,
                s.WorkspaceId,
                s.Name,
                s.IsPrivate,
                s.Color,
                s.AvatarUrl,
                s.Members.Count,
                s.CreatedAt))
            .ToListAsync(ct);
    }
}