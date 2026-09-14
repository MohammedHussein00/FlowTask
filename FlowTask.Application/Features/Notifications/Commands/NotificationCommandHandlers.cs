namespace FlowTask.Application.Features.Notifications.Commands;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Notifications.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.SystemOps;

public class CreateNotificationCommandHandler
    : IRequestHandler<CreateNotificationCommand, NotificationDto>
{
    private readonly IApplicationDbContext _db;

    public CreateNotificationCommandHandler(IApplicationDbContext db) => _db = db;

    public async Task<NotificationDto> Handle(
        CreateNotificationCommand request, CancellationToken ct)
    {
        var notification = new Notification
        {
            UserId     = request.UserId,
            Title      = request.Title,
            Message    = request.Message,
            EntityType = request.EntityType,
            EntityId   = request.EntityId,
            IsRead     = false
        };

        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync(ct);

        return new NotificationDto(
            notification.Id, notification.UserId, notification.Title,
            notification.Message, notification.IsRead,
            notification.EntityType, notification.EntityId,
            notification.CreatedAt);
    }
}

public class MarkNotificationReadCommandHandler
    : IRequestHandler<MarkNotificationReadCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public MarkNotificationReadCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(MarkNotificationReadCommand request, CancellationToken ct)
    {
        var userId       = _currentUser.UserId ?? throw new UnauthorizedException();
        var notification = await _db.Notifications
            .FirstOrDefaultAsync(n => n.Id == request.NotificationId && n.UserId == userId, ct)
            ?? throw new NotFoundException(localizationKey: "NotificationNotFound", args: [request.NotificationId], devMessage: $"Notification with id '{request.NotificationId}' was not found.");

        notification.IsRead = true;
        await _db.SaveChangesAsync(ct);
        return true;
    }
}

public class MarkAllNotificationsReadCommandHandler
    : IRequestHandler<MarkAllNotificationsReadCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public MarkAllNotificationsReadCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(MarkAllNotificationsReadCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        await _db.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true), ct);

        return true;
    }
}

public class DeleteNotificationCommandHandler
    : IRequestHandler<DeleteNotificationCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService   _currentUser;

    public DeleteNotificationCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db          = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(DeleteNotificationCommand request, CancellationToken ct)
    {
        var userId       = _currentUser.UserId ?? throw new UnauthorizedException();
        var notification = await _db.Notifications
            .FirstOrDefaultAsync(n => n.Id == request.NotificationId && n.UserId == userId, ct)
            ?? throw new NotFoundException(localizationKey: "NotificationNotFound", args: [request.NotificationId], devMessage: $"Notification with id '{request.NotificationId}' was not found.");

        _db.Notifications.Remove(notification);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}