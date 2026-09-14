namespace FlowTask.Application.Features.Lists.DTOs;

public record ListDto(
    int      Id,
    int?     FolderId,
    int      SpaceId,
    string   Name,
    string?  Content,
    string   Priority,
    DateTime? StartDate,
    DateTime? DueDate,
    int      TaskCount,
    DateTime CreatedAt
);