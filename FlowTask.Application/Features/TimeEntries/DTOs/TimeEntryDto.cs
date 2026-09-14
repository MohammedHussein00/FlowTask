namespace FlowTask.Application.Features.TimeEntries.DTOs;

public record TimeEntryDto(
    int      Id,
    int      TaskId,
    string   TaskName,
    int      UserId,
    string   UserName,
    string?  Description,
    DateTime StartTime,
    DateTime EndTime,
    int      Duration,
    DateTime CreatedAt
);