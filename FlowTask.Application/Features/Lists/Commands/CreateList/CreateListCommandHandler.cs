namespace FlowTask.Application.Features.Lists.Commands.CreateList;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Lists.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Hierarchy;

public class CreateListCommandHandler : IRequestHandler<CreateListCommand, ListDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public CreateListCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<ListDto> Handle(CreateListCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var space = await _db.Spaces
            .Include(s => s.Members)
            .FirstOrDefaultAsync(s => s.Id == request.SpaceId, ct)
            ?? throw new NotFoundException(localizationKey: "SpaceNotFound", args: [request.SpaceId],
                devMessage: $"Space with id '{request.SpaceId}' was not found.");

        var isMember = space.Members.Any(m => m.UserId == userId);
        if (!isMember)
            throw new UnauthorizedException("You are not a member of this space.");

        if (request.FolderId.HasValue)
        {
            var folderExists = await _db.Folders
                .AnyAsync(f => f.Id == request.FolderId && f.SpaceId == request.SpaceId, ct);
            if (!folderExists)
                throw new NotFoundException(localizationKey: "FolderNotFound", args: [request.FolderId],
                devMessage: $"Folder with id '{request.FolderId}' was not found.");
        }

        var list = new List
        {
            SpaceId   = request.SpaceId,
            FolderId  = request.FolderId,
            Name      = request.Name,
            Content   = request.Content,
            Priority  = request.Priority,
            StartDate = request.StartDate,
            DueDate   = request.DueDate
        };

        _db.Lists.Add(list);
        await _db.SaveChangesAsync(ct);

        return new ListDto(
            list.Id, list.FolderId, list.SpaceId, list.Name,
            list.Content, list.Priority, list.StartDate, list.DueDate,
            0, list.CreatedAt);
    }
}