namespace FlowTask.Api.GraphQL.Mutations.Ai;

using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;
using FlowTask.Application.Features.Ai.Commands;
using FlowTask.Application.Features.Ai.DTOs;

[ExtendObjectType("Mutation")]
public class AiMutation
{
    [Authorize]
    public async Task<AiAgentDto> CreateAiAgent(
        int workspaceId, string name, string purpose,
        string agentType, string? description,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateAiAgentCommand(
            workspaceId, name, purpose, agentType, description), ct);

    [Authorize]
    public async Task<AiPromptResponseDto> SendAiPrompt(
        int workspaceId, string promptInput,
        string modelName, string? contextType, int? contextId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new SendAiPromptCommand(
            workspaceId, promptInput, modelName, contextType, contextId), ct);

    [Authorize]
    public async Task<bool> RateAiResponse(
        int aiPromptResponseId, int rating, string? comments,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new RateAiResponseCommand(
            aiPromptResponseId, rating, comments), ct);

    [Authorize]
    public async Task<AiModelConfigDto> ConfigureAiModel(
        int workspaceId, string modelName, string modelProvider,
        decimal temperature, int maxTokens, bool isDefault,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ConfigureAiModelCommand(
            workspaceId, modelName, modelProvider,
            temperature, maxTokens, isDefault), ct);

    [Authorize]
    public async Task<bool> ToggleAiAgent(
        int agentId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ToggleAiAgentCommand(agentId), ct);
}