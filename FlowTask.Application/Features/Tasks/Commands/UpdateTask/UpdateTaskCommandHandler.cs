namespace FlowTask.Application.Features.Tasks.Commands.UpdateTask;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Tasks.DTOs;
using FlowTask.Application.Interfaces;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public UpdateTaskCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var task = await _db.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.Id, ct);

        if (task == null)
        {
            throw new NotFoundException(localizationKey: "TaskNotFound", args: [request.Id], devMessage: $"Task with id '{request.Id}' was not found.");
        }

        task.Name = request.Name;
        task.Description = request.Description;
        task.Priority = request.Priority;
        task.StatusId = request.StatusId;
        task.StartDate = request.StartDate;
        task.DueDate = request.DueDate;
        task.TimeEstimate = request.TimeEstimate;

        await _db.SaveChangesAsync(ct);

        return await _db.Tasks
            .Where(t => t.Id == task.Id)
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
            .FirstAsync(ct);
    }
}