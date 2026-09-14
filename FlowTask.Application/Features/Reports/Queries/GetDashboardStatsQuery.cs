namespace FlowTask.Application.Features.Reports.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Reports.DTOs;
using FlowTask.Application.Interfaces;

public record GetDashboardStatsQuery(int WorkspaceId) : IRequest<DashboardStatsDto>;

public class GetDashboardStatsQueryHandler
    : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public GetDashboardStatsQueryHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<DashboardStatsDto> Handle(
        GetDashboardStatsQuery request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var isMember = await _db.WorkspaceMembers
            .AnyAsync(m => m.WorkspaceId == request.WorkspaceId && m.UserId == userId, ct);

        if (!isMember)
            throw new UnauthorizedException("You are not a member of this workspace.");

        var now = DateTime.UtcNow;

        // All tasks in workspace
        var allTasks = await _db.Tasks
            .Where(t => t.List.Space.WorkspaceId == request.WorkspaceId)
            .Select(t => new
            {
                t.Id, t.DateClosed, t.DueDate, t.Priority,
                StatusName  = t.Status != null ? t.Status.Name  : "No Status",
                StatusColor = t.Status != null ? t.Status.Color : "#95A5A6",
                CreatorId   = t.CreatorId
            })
            .ToListAsync(ct);

        var total        = allTasks.Count;
        var completed    = allTasks.Count(t => t.DateClosed != null);
        var inProgress   = allTasks.Count(t => t.DateClosed == null &&
                                               t.StatusName.ToLower().Contains("progress"));
        var overdue      = allTasks.Count(t => t.DateClosed == null &&
                                               t.DueDate < now);

        var tasksByStatus = allTasks
            .GroupBy(t => new { t.StatusName, t.StatusColor })
            .Select(g => new TasksByStatusDto(g.Key.StatusName, g.Key.StatusColor, g.Count()))
            .OrderByDescending(s => s.Count)
            .ToList();

        var tasksByPriority = allTasks
            .GroupBy(t => t.Priority)
            .Select(g => new TasksByPriorityDto(g.Key, g.Count()))
            .OrderByDescending(p => p.Count)
            .ToList();

        var memberCount = await _db.WorkspaceMembers
            .CountAsync(m => m.WorkspaceId == request.WorkspaceId, ct);

        var spaceCount = await _db.Spaces
            .CountAsync(s => s.WorkspaceId == request.WorkspaceId, ct);

        var activeSprints = await _db.Sprints
            .CountAsync(s => s.List.Space.WorkspaceId == request.WorkspaceId
                          && s.Status == "active", ct);

        var topMembers = await _db.WorkspaceMembers
            .Where(m => m.WorkspaceId == request.WorkspaceId)
            .Select(m => new MemberActivityDto(
                m.UserId,
                m.User.FullName ?? m.User.UserName!,
                m.User.AvatarUrl,
                m.User.CreatedTasks.Count(t =>
                    t.List.Space.WorkspaceId == request.WorkspaceId &&
                    t.DateClosed != null),
                m.User.Comments.Count(c =>
                    c.Task.List.Space.WorkspaceId == request.WorkspaceId)))
            .OrderByDescending(m => m.TasksCompleted)
            .Take(5)
            .ToListAsync(ct);

        return new DashboardStatsDto(
            request.WorkspaceId,
            total, completed, inProgress, overdue,
            memberCount, spaceCount, activeSprints,
            total == 0 ? 0 : Math.Round((double)completed / total * 100, 1),
            tasksByStatus, tasksByPriority, topMembers);
    }
}