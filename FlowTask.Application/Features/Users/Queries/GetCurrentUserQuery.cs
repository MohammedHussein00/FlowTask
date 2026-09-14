namespace FlowTask.Application.Features.Users.Queries;

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Users.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Identity;

public record GetCurrentUserQuery : IRequest<UserProfileDto>;
public record GetUserByIdQuery(int Id) : IRequest<UserDto>;
public record GetWorkspaceMembersQuery(int WorkspaceId) : IRequest<List<UserDto>>;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserProfileDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUserService  _currentUser;

    public GetCurrentUserQueryHandler(UserManager<AppUser> userManager, ICurrentUserService currentUser)
    {
        _userManager = userManager;
        _currentUser = currentUser;
    }

    public async Task<UserProfileDto> Handle(GetCurrentUserQuery request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();
        var user   = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new 
                      NotFoundException(localizationKey: "UserNotFound", args: [userId], devMessage: $"User with id '{userId}' was not found.")
;

        return new UserProfileDto(
            user.Id, user.Email!, user.FullName ?? "",
            user.AvatarUrl, user.Timezone,
            user.TwoFactorEnabled, user.UserName!);
    }
}

public class GetWorkspaceMembersQueryHandler
    : IRequestHandler<GetWorkspaceMembersQuery, List<UserDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly UserManager<AppUser>  _userManager;
    private readonly ICurrentUserService   _currentUser;

    public GetWorkspaceMembersQueryHandler(
        IApplicationDbContext db,
        UserManager<AppUser>  userManager,
        ICurrentUserService   currentUser)
    {
        _db          = db;
        _userManager = userManager;
        _currentUser = currentUser;
    }

    public async Task<List<UserDto>> Handle(GetWorkspaceMembersQuery request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var isMember = await _db.WorkspaceMembers
            .AnyAsync(m => m.WorkspaceId == request.WorkspaceId && m.UserId == userId, ct);

        if (!isMember)
            throw new UnauthorizedException("You are not a member of this workspace.");

        var members = await _db.WorkspaceMembers
            .Where(m => m.WorkspaceId == request.WorkspaceId)
            .Select(m => new
            {
                m.User.Id,
                m.User.Email,
                FullName  = m.User.FullName ?? m.User.UserName!,
                m.User.AvatarUrl,
                m.User.Timezone,
                m.User.CreatedAtUtc
            })
            .ToListAsync(ct);

        return members.Select(m => new UserDto(
            m.Id, m.Email!, m.FullName, m.AvatarUrl, m.Timezone,
            [], m.CreatedAtUtc)).ToList();
    }
}