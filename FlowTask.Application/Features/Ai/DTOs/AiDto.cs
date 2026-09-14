namespace FlowTask.Application.Features.Ai.DTOs;

public record AiAgentDto(
    int      Id,
    int      WorkspaceId,
    string   Name,
    string?  Description,
    string   AgentType,
    string   Purpose,
    bool     Enabled,
    int?      CreatedBy,
    string   CreatedByName,
    DateTime CreatedAt
);

public record AiPromptResponseDto(
    int      Id,
    string   PromptInput,
    string   ModelResponse,
    string   ModelUsed,
    string?  UserFeedback,
    DateTime CreatedAt
);

public record AiModelConfigDto(
    int     Id,
    int     WorkspaceId,
    string  ModelName,
    string  ModelProvider,
    decimal Temperature,
    int     MaxTokens,
    bool    IsDefault,
    bool    Enabled
);