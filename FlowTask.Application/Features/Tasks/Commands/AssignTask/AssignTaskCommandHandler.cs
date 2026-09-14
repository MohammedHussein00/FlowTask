namespace FlowTask.Application.Features.Tasks.Commands.AssignTask;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Tasks;

public class AssignTaskCommandHandler : IRequestHandler<AssignTaskCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public AssignTaskCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(AssignTaskCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var task = await _db.Tasks.FindAsync([request.TaskId], ct);

        if (task == null)
        {
            throw new 
               NotFoundException(localizationKey: "TaskNotFound", args: [request.TaskId], devMessage: $"Task with id '{request.TaskId}' was not found.")
;
        }

        var alreadyAssigned = await _db.TaskAssignees
            .AnyAsync(a => a.TaskId == request.TaskId && a.UserId == request.UserId, ct);

        if (!alreadyAssigned)
        {
            _db.TaskAssignees.Add(new TaskAssignee
            {
                TaskId = request.TaskId,
                UserId = request.UserId
            });
            await _db.SaveChangesAsync(ct);
        }

        return true;
    }
}

public class UnassignTaskCommandHandler : IRequestHandler<UnassignTaskCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public UnassignTaskCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(UnassignTaskCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var assignee = await _db.TaskAssignees
            .FirstOrDefaultAsync(a => a.TaskId == request.TaskId && a.UserId == request.UserId, ct);

        if (assignee is not null)
        {
            _db.TaskAssignees.Remove(assignee);
            await _db.SaveChangesAsync(ct);
        }

        return true;
    }
}