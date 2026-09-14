namespace FlowTask.Application.Features.Goals.DTOs;

public record GoalDto(
    int      Id,
    int      WorkspaceId,
    string   Name,
    string?  Description,
    int?      OwnerId,
    string   OwnerName,
    DateTime? DueDate,
    string   Color,
    double   ProgressPercent,
    List<KeyResultDto> KeyResults,
    DateTime CreatedAt
);

public record KeyResultDto(
    int     Id,
    int     GoalId,
    string  Name,
    string  ResultType,
    decimal? TargetValue,
    decimal CurrentValue,
    int?     OwnerId,
    string  OwnerName,
    double  ProgressPercent
);