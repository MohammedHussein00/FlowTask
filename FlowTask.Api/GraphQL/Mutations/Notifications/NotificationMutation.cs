namespace FlowTask.Api.GraphQL.Mutations.Notifications;

using FlowTask.Application.Features.Notifications.Commands;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Mutation")]
public class NotificationMutation
{
    [Authorize]
    public async Task<bool> MarkNotificationRead(
        int notificationId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new MarkNotificationReadCommand(notificationId), ct);

    [Authorize]
    public async Task<bool> MarkAllNotificationsRead(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new MarkAllNotificationsReadCommand(), ct);

    [Authorize]
    public async Task<bool> DeleteNotification(
        int notificationId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteNotificationCommand(notificationId), ct);
}