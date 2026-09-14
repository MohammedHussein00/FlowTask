namespace FlowTask.Application.Features.Notifications.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Notifications.DTOs;
using FlowTask.Application.Interfaces;

public record GetMyNotificationsQuery(
    bool UnreadOnly = false,
    int  Page       = 1,
    int  PageSize   = 20
) : IRequest<List<NotificationDto>>;

public record GetNotificationCountQuery : IRequest<NotificationCountDto>;

public class GetMyNotificationsQueryHandler
    : IRequestHandler<GetMyNotificationsQuery, List<NotificationDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public GetMyNotificationsQueryHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<List<NotificationDto>> Handle(
        GetMyNotificationsQuery request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var query = _db.Notifications.Where(n => n.UserId == userId);

        if (request.UnreadOnly)
            query = query.Where(n => !n.IsRead);

        return await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(n => new NotificationDto(
                n.Id, n.UserId, n.Title, n.Message,
                n.IsRead, n.EntityType, n.EntityId, n.CreatedAt))
            .ToListAsync(ct);
    }
}

public class GetNotificationCountQueryHandler
    : IRequestHandler<GetNotificationCountQuery, NotificationCountDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public GetNotificationCountQueryHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<NotificationCountDto> Handle(
        GetNotificationCountQuery request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var total  = await _db.Notifications.CountAsync(n => n.UserId == userId, ct);
        var unread = await _db.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead, ct);

        return new NotificationCountDto(total, unread);
    }
}