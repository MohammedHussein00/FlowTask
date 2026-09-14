namespace FlowTask.Application.Features.Sprints.Commands;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Sprints.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Sprints;

public class CreateSprintCommandHandler : IRequestHandler<CreateSprintCommand, SprintDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CreateSprintCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<SprintDto> Handle(CreateSprintCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        if (request.EndDate <= request.StartDate)
        {
            throw new ValidationException(
             new Dictionary<string, IReadOnlyList<(string, object[])>>
             {
                 ["EndDate"] = new List<(string, object[])>
                 {
            ("EndDateAfterStartDate", Array.Empty<object>())
                 }
             });
        }

        var listExists = await _db.Lists.AnyAsync(l => l.Id == request.ListId, ct);

        if (!listExists)
        {
            throw new NotFoundException(localizationKey: "ListNotFound", args: [request.ListId], devMessage: $"List with id '{request.ListId}' was not found.");
        }

        var sprint = new Sprint
        {
            ListId = request.ListId,
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Goal = request.Goal,
            Status = "planned"
        };

        _db.Sprints.Add(sprint);
        await _db.SaveChangesAsync(ct);

        return new SprintDto(
            sprint.Id,
            sprint.ListId,
            sprint.Name,
            sprint.Description,
            sprint.StartDate,
            sprint.EndDate,
            sprint.Status,
            sprint.Goal,
            0,
            0,
            sprint.CreatedAt);
    }
}

public class UpdateSprintStatusCommandHandler : IRequestHandler<UpdateSprintStatusCommand, SprintDto>
{
    private static readonly string[] ValidStatuses = ["planned", "active", "completed", "cancelled"];
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public UpdateSprintStatusCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<SprintDto> Handle(UpdateSprintStatusCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        if (!ValidStatuses.Contains(request.Status))
        {
            throw new ValidationException(
                new Dictionary<string, IReadOnlyList<(string, object[])>>
                {
                    ["Status"] = new List<(string, object[])>
                    {
            ("StatusInvalid", new object[] { string.Join(", ", ValidStatuses) })
                    }
                });
        }

        var sprint = await _db.Sprints
            .Include(s => s.SprintTasks)
            .FirstOrDefaultAsync(s => s.Id == request.SprintId, ct);

        if (sprint == null)
        {
            throw new 
                   NotFoundException(localizationKey: "SprintNotFound", args: [request.SprintId], devMessage: $"Sprint with id '{request.SprintId}' was not found.")
;
        }

        sprint.Status = request.Status;
        await _db.SaveChangesAsync(ct);

        var completedCount = sprint.SprintTasks
            .Count(st => st.Task?.DateClosed != null);

        return new SprintDto(
            sprint.Id,
            sprint.ListId,
            sprint.Name,
            sprint.Description,
            sprint.StartDate,
            sprint.EndDate,
            sprint.Status,
            sprint.Goal,
            sprint.SprintTasks.Count,
            completedCount,
            sprint.CreatedAt);
    }
}

public class AddTaskToSprintCommandHandler : IRequestHandler<AddTaskToSprintCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public AddTaskToSprintCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(AddTaskToSprintCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var sprintExists = await _db.Sprints.AnyAsync(s => s.Id == request.SprintId, ct);

        if (!sprintExists)
        {
            throw new 
                NotFoundException(localizationKey: "SprintNotFound", args: [request.SprintId], devMessage: $"Sprint with id '{request.SprintId}' was not found.")
                ;
        }

        var taskExists = await _db.Tasks.AnyAsync(t => t.Id == request.TaskId, ct);

        if (!taskExists)
        {
            throw new
                NotFoundException(localizationKey: "TaskNotFound", args: [request.TaskId], devMessage: $"Task with id '{request.TaskId}' was not found.")
                ;
        }

        var alreadyAdded = await _db.SprintTasks
            .AnyAsync(st => st.SprintId == request.SprintId && st.TaskId == request.TaskId, ct);

        if (!alreadyAdded)
        {
            _db.SprintTasks.Add(new SprintTask
            {
                SprintId = request.SprintId,
                TaskId = request.TaskId,
                StoryPoints = request.StoryPoints
            });
            await _db.SaveChangesAsync(ct);
        }

        return true;
    }
}

public class RemoveTaskFromSprintCommandHandler : IRequestHandler<RemoveTaskFromSprintCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public RemoveTaskFromSprintCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(RemoveTaskFromSprintCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var sprintTask = await _db.SprintTasks
            .FirstOrDefaultAsync(st =>
                st.SprintId == request.SprintId &&
                st.TaskId == request.TaskId, ct);

        if (sprintTask == null)
        {
            throw new NotFoundException("SprintTask", $"{request.SprintId}-{request.TaskId}");
        }

        _db.SprintTasks.Remove(sprintTask);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}