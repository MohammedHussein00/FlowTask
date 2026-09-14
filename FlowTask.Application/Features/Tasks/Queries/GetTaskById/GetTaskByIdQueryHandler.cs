namespace FlowTask.Application.Features.Tasks.Queries.GetTaskById;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Tasks.DTOs;
using FlowTask.Application.Interfaces;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public GetTaskByIdQueryHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var task = await _db.Tasks
            .Where(t => t.Id == request.Id)
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
            .FirstOrDefaultAsync(ct);

        if (task == null)
        {
            throw new NotFoundException(localizationKey: "TaskNotFound", args: [request.Id], devMessage: $"Task with id '{request.Id}' was not found.");
        }

        return task;
    }
}