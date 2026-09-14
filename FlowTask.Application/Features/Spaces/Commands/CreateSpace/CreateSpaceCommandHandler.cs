
namespace FlowTask.Application.Features.Spaces.Commands.CreateSpace;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Spaces.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Hierarchy;

public class CreateSpaceCommandHandler : IRequestHandler<CreateSpaceCommand, SpaceDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public CreateSpaceCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<SpaceDto> Handle(CreateSpaceCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var isMember = await _db.WorkspaceMembers
            .AnyAsync(m => m.WorkspaceId == request.WorkspaceId && m.UserId == userId, ct);

        if (!isMember)
            throw new UnauthorizedException("You are not a member of this workspace.");

        var space = new Space
        {
            WorkspaceId = request.WorkspaceId,
            Name        = request.Name,
            IsPrivate   = request.IsPrivate,
            Color       = request.Color,
            AvatarUrl   = request.AvatarUrl
        };

        _db.Spaces.Add(space);

        _db.SpaceMembers.Add(new SpaceMember
        {
            Space       = space,
            UserId      = userId,
            AccessLevel = "admin"
        });

        await _db.SaveChangesAsync(ct);

        return new SpaceDto(
            space.Id, space.WorkspaceId, space.Name,
            space.IsPrivate, space.Color, space.AvatarUrl,
            1, space.CreatedAt);
    }
}