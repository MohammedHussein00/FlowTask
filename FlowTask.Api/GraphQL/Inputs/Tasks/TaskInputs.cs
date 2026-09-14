namespace FlowTask.Api.GraphQL.Inputs.Tasks;

public record CreateTaskInput(
    int      ListId,
    string   Name,
    string?  Description  = null,
    string   Priority     = "normal",
    int?     StatusId     = null,
    int?     ParentId     = null,
    DateTime? StartDate   = null,
    DateTime? DueDate     = null,
    int?     TimeEstimate = null,
    List<int>? AssigneeIds = null
);

public record UpdateTaskInput(
    int      Id,
    string   Name,
    string?  Description  = null,
    string   Priority     = "normal",
    int?     StatusId     = null,
    DateTime? StartDate   = null,
    DateTime? DueDate     = null,
    int?     TimeEstimate = null
);

public record AssignTaskInput(int TaskId, int UserId);