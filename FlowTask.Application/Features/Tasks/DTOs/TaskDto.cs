
namespace FlowTask.Application.Features.Tasks.DTOs;

public record TaskDto(
    int      Id,
    int      ListId,
    string   Name,
    string?  Description,
    string   Priority,
    string?  StatusName,
    string?  StatusColor,
    int      CreatorId,
    string   CreatorName,
    int?     ParentId,
    DateTime? StartDate,
    DateTime? DueDate,
    DateTime? DateClosed,
    int?     TimeEstimate,
    List<AssigneeDto> Assignees,
    DateTime CreatedAt
);

public record AssigneeDto(int UserId, string FullName, string? AvatarUrl);