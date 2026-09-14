namespace FlowTask.Api.GraphQL.Types.Tasks;

using FlowTask.Application.Features.Tasks.DTOs;

public class TaskType
{
    public int       Id           { get; init; }
    public int       ListId       { get; init; }
    public string    Name         { get; init; } = default!;
    public string?   Description  { get; init; }
    public string    Priority     { get; init; } = default!;
    public string?   StatusName   { get; init; }
    public string?   StatusColor  { get; init; }
    public int       CreatorId    { get; init; }
    public string    CreatorName  { get; init; } = default!;
    public int?      ParentId     { get; init; }
    public DateTime? StartDate    { get; init; }
    public DateTime? DueDate      { get; init; }
    public DateTime? DateClosed   { get; init; }
    public int?      TimeEstimate { get; init; }
    public List<AssigneeType> Assignees { get; init; } = [];
    public DateTime  CreatedAt    { get; init; }

    public static TaskType FromDto(TaskDto dto) => new()
    {
        Id           = dto.Id,
        ListId       = dto.ListId,
        Name         = dto.Name,
        Description  = dto.Description,
        Priority     = dto.Priority,
        StatusName   = dto.StatusName,
        StatusColor  = dto.StatusColor,
        CreatorId    = dto.CreatorId,
        CreatorName  = dto.CreatorName,
        ParentId     = dto.ParentId,
        StartDate    = dto.StartDate,
        DueDate      = dto.DueDate,
        DateClosed   = dto.DateClosed,
        TimeEstimate = dto.TimeEstimate,
        Assignees    = dto.Assignees.Select(a => new AssigneeType(a.UserId, a.FullName, a.AvatarUrl)).ToList(),
        CreatedAt    = dto.CreatedAt
    };
}

public record AssigneeType(int UserId, string FullName, string? AvatarUrl);