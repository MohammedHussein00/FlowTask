namespace FlowTask.Api.GraphQL.Queries.Ai;

using FlowTask.Application.Features.Ai.DTOs;
using FlowTask.Application.Interfaces;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

[ExtendObjectType("Query")]
public class AiQuery
{
    [Authorize]
    public async Task<List<AiAgentDto>> GetAiAgents(
        int workspaceId,
        [Service] IApplicationDbContext db,
        CancellationToken ct)
        => await db.AiAgents
            .Where(a => a.WorkspaceId == workspaceId)
            .Select(a => new AiAgentDto(
                a.Id, a.WorkspaceId, a.Name, a.Description,
                a.AgentType, a.Purpose, a.Enabled,
                a.CreatedBy,
                a.CreatedByUser.FullName ?? a.CreatedByUser.UserName!,
                a.CreatedAt))
            .ToListAsync(ct);

    [Authorize]
    public async Task<List<AiModelConfigDto>> GetAiModelConfigs(
        int workspaceId,
        [Service] IApplicationDbContext db,
        CancellationToken ct)
        => await db.AiModelConfigs
            .Where(c => c.WorkspaceId == workspaceId && c.Enabled)
            .Select(c => new AiModelConfigDto(
                c.Id, c.WorkspaceId, c.ModelName, c.ModelProvider,
                c.Temperature, c.MaxTokens, c.IsDefault, c.Enabled))
            .ToListAsync(ct);

    [Authorize]
    public async Task<List<AiPromptResponseDto>> GetMyAiHistory(
        int workspaceId,
        [Service] IApplicationDbContext db,
        [Service] ICurrentUserService currentUser,
        CancellationToken ct)
    {
        var userId = currentUser.UserId ?? 0;
        return await db.AiPromptResponses
            .Where(p => p.WorkspaceId == workspaceId && p.UserId == userId)
            .Select(p => new AiPromptResponseDto(
                p.Id, p.PromptInput, p.ModelResponse,
                p.ModelUsed, p.UserFeedback, p.CreatedAt))
            .OrderByDescending(p => p.CreatedAt)
            .Take(50)
            .ToListAsync(ct);
    }
}