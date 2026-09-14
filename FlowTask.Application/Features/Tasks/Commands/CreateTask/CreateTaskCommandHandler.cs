namespace FlowTask.Application.Features.Tasks.Commands.CreateTask;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Tasks.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Tasks;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public CreateTaskCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var list = await _db.Lists.FindAsync([request.ListId], ct)
            ?? throw new 
                        NotFoundException(localizationKey: "ListNotFound", args: [request.ListId], devMessage: $"List with id '{request.ListId}' was not found.");

        var task = new Domain.Tasks.Task
        {
            ListId       = request.ListId,
            Name         = request.Name,
            Description  = request.Description,
            Priority     = request.Priority,
            StatusId     = request.StatusId,
            ParentId     = request.ParentId,
            StartDate    = request.StartDate,
            DueDate      = request.DueDate,
            TimeEstimate = request.TimeEstimate,
            CreatorId    = userId
        };

        _db.Tasks.Add(task);

        if (request.AssigneeIds?.Count > 0)
        {
            foreach (var assigneeId in request.AssigneeIds.Distinct())
            {
                _db.TaskAssignees.Add(new TaskAssignee
                {
                    Task   = task,
                    UserId = assigneeId
                });
            }
        }

        await _db.SaveChangesAsync(ct);

        return await ProjectToDto(task.Id, ct);
    }

    private async Task<TaskDto> ProjectToDto(int taskId, CancellationToken ct)
    {
        return await _db.Tasks
            .Where(t => t.Id == taskId)
            .Select(t => new TaskDto(
                t.Id,
                t.ListId,
                t.Name,
                t.Description,
                t.Priority,
                t.Status != null ? t.Status.Name  : null,
                t.Status != null ? t.Status.Color : null,
                t.CreatorId,
                t.Creator.FullName ?? t.Creator.UserName!,
                t.ParentId,
                t.StartDate,
                t.DueDate,
                t.DateClosed,
                t.TimeEstimate,
                t.Assignees.Select(a => new AssigneeDto(
                    a.UserId,
                    a.User.FullName ?? a.User.UserName!,
                    a.User.AvatarUrl)).ToList(),
                t.CreatedAt))
            .FirstAsync(ct);
    }
}