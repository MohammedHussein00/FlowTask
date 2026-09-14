namespace FlowTask.Application.Features.Automations.DTOs;

public record AutomationDto(
    int      Id,
    int      WorkspaceId,
    int?     SpaceId,
    int?     ListId,
    string   Name,
    string?  Description,
    bool     IsActive,
    string   TriggerType,
    string?  ConditionLogic,
    int?      CreatedBy,
    string   CreatedByName,
    List<TriggerDto> Triggers,
    List<ActionDto>  Actions,
    DateTime CreatedAt
);

public record TriggerDto(
    int    Id,
    string TriggerEvent,
    string EntityType,
    string? Conditions,
    bool   IsActive
);

public record ActionDto(
    int     Id,
    string  ActionType,
    string? ActionConfig,
    int?    OrderIndex,
    bool    IsActive
);