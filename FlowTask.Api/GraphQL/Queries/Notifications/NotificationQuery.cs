namespace FlowTask.Api.GraphQL.Queries.Notifications;

using FlowTask.Application.Features.Notifications.DTOs;
using FlowTask.Application.Features.Notifications.Queries;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Query")]
public class NotificationQuery
{
    [Authorize]
    public async Task<List<NotificationDto>> GetMyNotifications(
        bool unreadOnly, int page, int pageSize,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(
            new GetMyNotificationsQuery(unreadOnly, page, pageSize), ct);

    [Authorize]
    public async Task<NotificationCountDto> GetNotificationCount(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetNotificationCountQuery(), ct);
}