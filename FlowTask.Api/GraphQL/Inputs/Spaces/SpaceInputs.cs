namespace FlowTask.Api.GraphQL.Inputs.Spaces;

public record CreateSpaceInput(
    int     WorkspaceId,
    string  Name,
    bool    IsPrivate = false,
    string  Color     = "#5B9BD5",
    string? AvatarUrl = null
);

public record UpdateSpaceInput(
    int     Id,
    string  Name,
    bool    IsPrivate = false,
    string  Color     = "#5B9BD5",
    string? AvatarUrl = null
);