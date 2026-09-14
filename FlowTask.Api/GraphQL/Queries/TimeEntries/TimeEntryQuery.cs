namespace FlowTask.Api.GraphQL.Queries.TimeEntries;

using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Interfaces;
using FlowTask.Application.Features.TimeEntries.DTOs;

[ExtendObjectType("Query")]
public class TimeEntryQuery
{
    [Authorize]
    public async Task<List<TimeEntryDto>> GetTimeEntriesByTask(
        int taskId,
        [Service] IApplicationDbContext db,
        CancellationToken ct)
        => await db.TimeEntries
            .Where(e => e.TaskId == taskId)
            .Select(e => new TimeEntryDto(
                e.Id, e.TaskId, e.Task.Name,
                e.UserId, e.User.FullName ?? e.User.UserName!,
                e.Description, e.StartTime, e.EndTime, e.Duration,
                DateTime.UtcNow))
            .OrderByDescending(e => e.StartTime)
            .ToListAsync(ct);

    [Authorize]
    public async Task<List<TimeEntryDto>> GetMyTimeEntries(
        [Service] IApplicationDbContext db,
        [Service] ICurrentUserService currentUser,
        CancellationToken ct)
    {
        var userId = currentUser.UserId ?? 0;
        return await db.TimeEntries
            .Where(e => e.UserId == userId)
            .Select(e => new TimeEntryDto(
                e.Id, e.TaskId, e.Task.Name,
                e.UserId, e.User.FullName ?? e.User.UserName!,
                e.Description, e.StartTime, e.EndTime, e.Duration,
                DateTime.UtcNow))
            .OrderByDescending(e => e.StartTime)
            .ToListAsync(ct);
    }
}