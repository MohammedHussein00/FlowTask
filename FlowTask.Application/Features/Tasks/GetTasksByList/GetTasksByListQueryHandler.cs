namespace FlowTask.Application.Features.Tasks.Queries.GetTasksByList;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Tasks.DTOs;
using FlowTask.Application.Interfaces;

public class GetTasksByListQueryHandler : IRequestHandler<GetTasksByListQuery, List<TaskDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public GetTasksByListQueryHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<TaskDto>> Handle(GetTasksByListQuery request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        return await _db.Tasks
            .Where(t => t.ListId == request.ListId && t.ParentId == null)
            .Select(t => new TaskDto(
                t.Id,
                t.ListId,
                t.Name,
                t.Description,
                t.Priority,
                t.Status != null ? t.Status.Name : null,
                t.Status != null ? t.Status.Color : null,
                t.CreatorId,
                t.Creator.FullName ?? t.Creator.UserName,
                t.ParentId,
                t.StartDate,
                t.DueDate,
                t.DateClosed,
                t.TimeEstimate,
                t.Assignees.Select(a => new AssigneeDto(
                    a.UserId,
                    a.User.FullName ?? a.User.UserName,
                    a.User.AvatarUrl)).ToList(),
                t.CreatedAt))
            .OrderBy(t => t.CreatedAt)
            .ToListAsync(ct);
    }
}