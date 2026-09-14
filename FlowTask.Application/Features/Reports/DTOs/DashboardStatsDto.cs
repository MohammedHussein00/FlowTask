namespace FlowTask.Application.Features.Reports.DTOs;

public record DashboardStatsDto(
    int WorkspaceId,
    int TotalTasks,
    int CompletedTasks,
    int InProgressTasks,
    int OverdueTasks,
    int TotalMembers,
    int TotalSpaces,
    int ActiveSprints,
    double CompletionRate,
    List<TasksByStatusDto> TasksByStatus,
    List<TasksByPriorityDto> TasksByPriority,
    List<MemberActivityDto> TopActiveMembers
);

public record TasksByStatusDto(string StatusName, string Color, int Count);
public record TasksByPriorityDto(string Priority, int Count);
public record MemberActivityDto(int UserId, string UserName, string? AvatarUrl, int TasksCompleted, int CommentsCount);