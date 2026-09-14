namespace FlowTask.Application.Features.TimeEntries.Commands;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.TimeEntries.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Productivity;

public class LogTimeCommandHandler : IRequestHandler<LogTimeCommand, TimeEntryDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public LogTimeCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<TimeEntryDto> Handle(LogTimeCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        if (request.EndTime <= request.StartTime)
            throw new ValidationException(
               new Dictionary<string, IReadOnlyList<(string, object[])>>
               {
                   ["EndTime"] = new List<(string, object[])>
                   {
            ("EndTimeAfterStartTime", Array.Empty<object>())
                   }
               });

        var task = await _db.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, ct)
            ?? throw new NotFoundException(localizationKey: "TaskNotFound", args: [request.TaskId], devMessage: $"Task with id '{request.TaskId}' was not found.");

        var duration = (int)(request.EndTime - request.StartTime).TotalMinutes;

        var entry = new TimeEntry
        {
            TaskId      = request.TaskId,
            UserId      = userId,
            StartTime   = request.StartTime,
            EndTime     = request.EndTime,
            Duration    = duration,
            Description = request.Description
        };

        _db.TimeEntries.Add(entry);
        await _db.SaveChangesAsync(ct);

        return await _db.TimeEntries
            .Where(e => e.Id == entry.Id)
            .Select(e => new TimeEntryDto(
                e.Id, e.TaskId,
                e.Task.Name,
                e.UserId,
                e.User.FullName ?? e.User.UserName!,
                e.Description,
                e.StartTime, e.EndTime, e.Duration,
                DateTime.UtcNow))
            .FirstAsync(ct);
    }
}

public class DeleteTimeEntryCommandHandler : IRequestHandler<DeleteTimeEntryCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public DeleteTimeEntryCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(DeleteTimeEntryCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();
        var entry  = await _db.TimeEntries.FindAsync([request.Id], ct)
            ?? throw new 
             NotFoundException(localizationKey: "TimeEntryFound", args: [request.Id], devMessage: $"TimeEntry with id '{request.Id}' was not found.");

        if (entry.UserId != userId)
            throw new UnauthorizedException("You can only delete your own time entries.");

        _db.TimeEntries.Remove(entry);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}