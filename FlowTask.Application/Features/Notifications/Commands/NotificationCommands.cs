namespace FlowTask.Application.Features.Notifications.Commands;

using MediatR;
using FlowTask.Application.Features.Notifications.DTOs;

public record MarkNotificationReadCommand(int NotificationId) : IRequest<bool>;
public record MarkAllNotificationsReadCommand : IRequest<bool>;
public record DeleteNotificationCommand(int NotificationId) : IRequest<bool>;

// Internal command — fired by other handlers
public record CreateNotificationCommand(
    int     UserId,
    string  Title,
    string? Message     = null,
    string? EntityType  = null,
    int?    EntityId    = null
) : IRequest<NotificationDto>;