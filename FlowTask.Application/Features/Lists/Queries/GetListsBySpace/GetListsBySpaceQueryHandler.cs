namespace FlowTask.Application.Features.Lists.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Lists.DTOs;
using FlowTask.Application.Interfaces;

public class GetListsBySpaceQueryHandler : IRequestHandler<GetListsBySpaceQuery, List<ListDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public GetListsBySpaceQueryHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<ListDto>> Handle(GetListsBySpaceQuery request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        return await _db.Lists
            .Where(l => l.SpaceId == request.SpaceId)
            .Select(l => new ListDto(
                l.Id,
                l.FolderId,
                l.SpaceId,
                l.Name,
                l.Content,
                l.Priority,
                l.StartDate,
                l.DueDate,
                l.Tasks.Count,
                l.CreatedAt))
            .OrderBy(l => l.Name)
            .ToListAsync(ct);
    }
}