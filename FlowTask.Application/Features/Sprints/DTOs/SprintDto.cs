namespace FlowTask.Application.Features.Sprints.DTOs;

public record SprintDto(
    int      Id,
    int      ListId,
    string   Name,
    string?  Description,
    DateTime StartDate,
    DateTime EndDate,
    string   Status,
    string?  Goal,
    int      TotalTasks,
    int      CompletedTasks,
    DateTime CreatedAt
);