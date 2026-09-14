namespace FlowTask.Application.Features.Tasks.Commands.DeleteTask;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Interfaces;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public DeleteTaskCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var task = await _db.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.Id, ct)
            ?? throw new
            NotFoundException(localizationKey: "TaskNotFound", args: [request.Id], devMessage: $"Task with id '{request.Id}' was not found.")
;

        if (task.CreatorId != userId)
            throw new UnauthorizedException("Only the task creator can delete it.");

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}