namespace FlowTask.Application.Features.Spaces.Commands.UpdateSpace;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Spaces.DTOs;
using FlowTask.Application.Interfaces;

public class UpdateSpaceCommandHandler : IRequestHandler<UpdateSpaceCommand, SpaceDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public UpdateSpaceCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<SpaceDto> Handle(UpdateSpaceCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var space = await _db.Spaces
            .Include(s => s.Members)
            .FirstOrDefaultAsync(s => s.Id == request.Id, ct)
            ?? throw new NotFoundException(localizationKey: "SpaceNotFound", args: [request.Id], devMessage: $"Space with id '{request.Id}' was not found.");

        var isAdmin = space.Members
            .Any(m => m.UserId == userId && m.AccessLevel == "admin");

        if (!isAdmin)
            throw new UnauthorizedException("Only space admins can update this space.");

        space.Name      = request.Name;
        space.IsPrivate = request.IsPrivate;
        space.Color     = request.Color;
        space.AvatarUrl = request.AvatarUrl;

        await _db.SaveChangesAsync(ct);

        return new SpaceDto(
            space.Id, space.WorkspaceId, space.Name,
            space.IsPrivate, space.Color, space.AvatarUrl,
            space.Members.Count, space.CreatedAt);
    }
}