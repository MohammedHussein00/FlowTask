namespace FlowTask.Application.Features.Notifications.DTOs;

public record NotificationDto(
    int      Id,
    int      UserId,
    string   Title,
    string?  Message,
    bool     IsRead,
    string?  EntityType,
    int?     EntityId,
    DateTime CreatedAt
);

public record NotificationCountDto(int Total, int Unread);