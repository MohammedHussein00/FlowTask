namespace FlowTask.Application.Features.Ai.Commands;

using MediatR;
using FlowTask.Application.Features.Ai.DTOs;

public record CreateAiAgentCommand(
    int     WorkspaceId,
    string  Name,
    string  Purpose,
    string  AgentType    = "super_agent",
    string? Description  = null,
    string? TriggerConditions = null
) : IRequest<AiAgentDto>;

public record SendAiPromptCommand(
    int     WorkspaceId,
    string  PromptInput,
    string  ModelName    = "gpt-4o",
    string? ContextType  = null,
    int?    ContextId    = null
) : IRequest<AiPromptResponseDto>;

public record RateAiResponseCommand(
    int    AiPromptResponseId,
    int    Rating,
    string? Comments = null
) : IRequest<bool>;

public record ConfigureAiModelCommand(
    int     WorkspaceId,
    string  ModelName,
    string  ModelProvider,
    decimal Temperature = 0.7m,
    int     MaxTokens   = 2000,
    bool    IsDefault   = false
) : IRequest<AiModelConfigDto>;

public record ToggleAiAgentCommand(int AgentId) : IRequest<bool>;