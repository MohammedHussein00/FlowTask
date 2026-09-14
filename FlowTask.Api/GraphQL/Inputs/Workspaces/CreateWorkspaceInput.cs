namespace FlowTask.Api.GraphQL.Inputs.Workspaces;

public record CreateWorkspaceInput(string Name, string? AvatarUrl = null);
public record UpdateWorkspaceInput(int Id, string Name, string? AvatarUrl = null);