namespace FlowTask.Application.Features.Ai.Commands;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Ai.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Ai;

public class CreateAiAgentCommandHandler
    : IRequestHandler<CreateAiAgentCommand, AiAgentDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CreateAiAgentCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<AiAgentDto> Handle(
        CreateAiAgentCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        int? userId = _currentUser.UserId;

        var agent = new AiAgent
        {
            WorkspaceId = request.WorkspaceId,
            Name = request.Name,
            Description = request.Description,
            AgentType = request.AgentType,
            Purpose = request.Purpose,
            TriggerConditions = request.TriggerConditions,
            Enabled = true,
            CreatedBy = userId
        };

        _db.AiAgents.Add(agent);
        await _db.SaveChangesAsync(ct);

        return await _db.AiAgents
            .Where(a => a.Id == agent.Id)
            .Select(a => new AiAgentDto(
                a.Id,
                a.WorkspaceId,
                a.Name,
                a.Description,
                a.AgentType,
                a.Purpose,
                a.Enabled,
                a.CreatedBy,
                a.CreatedByUser.FullName ?? a.CreatedByUser.UserName,
                a.CreatedAt))
            .FirstAsync(ct);
    }
}

public class SendAiPromptCommandHandler
    : IRequestHandler<SendAiPromptCommand, AiPromptResponseDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public SendAiPromptCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<AiPromptResponseDto> Handle(
        SendAiPromptCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        int? userId = _currentUser.UserId;

        // NOTE: Wire up your real AI provider (OpenAI, Anthropic, etc.) here.
        // This is a stub response for architecture purposes.
        var modelResponse = $"[AI Response placeholder for: {request.PromptInput}]";

        var promptResponse = new AiPromptResponse
        {
            WorkspaceId = request.WorkspaceId,
            UserId = userId,
            PromptInput = request.PromptInput,
            ModelResponse = modelResponse,
            ModelUsed = request.ModelName,
            ContextType = request.ContextType,
            ContextId = request.ContextId
        };

        _db.AiPromptResponses.Add(promptResponse);

        // Track usage
        var usageDate = DateTime.UtcNow.Date;
        var usage = await _db.AiModelUsage
            .FirstOrDefaultAsync(u =>
                u.WorkspaceId == request.WorkspaceId &&
                u.ModelName == request.ModelName &&
                u.UsageDate == usageDate, ct);

        if (usage is null)
        {
            _db.AiModelUsage.Add(new AiModelUsage
            {
                WorkspaceId = request.WorkspaceId,
                ModelName = request.ModelName,
                UsageDate = usageDate,
                RequestCount = 1
            });
        }
        else
        {
            usage.RequestCount++;
        }

        await _db.SaveChangesAsync(ct);

        return new AiPromptResponseDto(
            promptResponse.Id,
            promptResponse.PromptInput,
            promptResponse.ModelResponse,
            promptResponse.ModelUsed,
            promptResponse.UserFeedback,
            promptResponse.CreatedAt);
    }
}

public class RateAiResponseCommandHandler
    : IRequestHandler<RateAiResponseCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public RateAiResponseCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(RateAiResponseCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        int? userId = _currentUser.UserId;

        var promptResponse = await _db.AiPromptResponses
            .FirstOrDefaultAsync(p => p.Id == request.AiPromptResponseId, ct);

        if (promptResponse == null)
        {
            throw new NotFoundException(
                localizationKey: "AiPromptResponseNotFound",
                args: [request.AiPromptResponseId],
                devMessage: $"AiPromptResponse with id '{request.AiPromptResponseId}' was not found.");
        }

        _db.AiFeedbackRatings.Add(new AiFeedbackRating
        {
            AiPromptResponseId = request.AiPromptResponseId,
            UserId = userId,
            Rating = request.Rating,
            Comments = request.Comments
        });

        await _db.SaveChangesAsync(ct);
        return true;
    }
}

public class ConfigureAiModelCommandHandler
    : IRequestHandler<ConfigureAiModelCommand, AiModelConfigDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public ConfigureAiModelCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<AiModelConfigDto> Handle(
        ConfigureAiModelCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        // If setting as default, un-default existing
        if (request.IsDefault)
        {
            await _db.AiModelConfigs
                .Where(c => c.WorkspaceId == request.WorkspaceId && c.IsDefault)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.IsDefault, false), ct);
        }

        var existing = await _db.AiModelConfigs
            .FirstOrDefaultAsync(c =>
                c.WorkspaceId == request.WorkspaceId &&
                c.ModelName == request.ModelName, ct);

        if (existing is not null)
        {
            existing.Temperature = request.Temperature;
            existing.MaxTokens = request.MaxTokens;
            existing.IsDefault = request.IsDefault;
            existing.ModelProvider = request.ModelProvider;
        }
        else
        {
            existing = new AiModelConfig
            {
                WorkspaceId = request.WorkspaceId,
                ModelName = request.ModelName,
                ModelProvider = request.ModelProvider,
                Temperature = request.Temperature,
                MaxTokens = request.MaxTokens,
                IsDefault = request.IsDefault,
                Enabled = true
            };
            _db.AiModelConfigs.Add(existing);
        }

        await _db.SaveChangesAsync(ct);

        return new AiModelConfigDto(
            existing.Id,
            existing.WorkspaceId,
            existing.ModelName,
            existing.ModelProvider,
            existing.Temperature,
            existing.MaxTokens,
            existing.IsDefault,
            existing.Enabled);
    }
}

public class ToggleAiAgentCommandHandler : IRequestHandler<ToggleAiAgentCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public ToggleAiAgentCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(ToggleAiAgentCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var agent = await _db.AiAgents.FindAsync([request.AgentId], ct);

        if (agent == null)
        {
            throw new NotFoundException(
                localizationKey: "AiAgentNotFound",
                args: [request.AgentId],
                devMessage: $"AiAgent with id '{request.AgentId}' was not found.");
        }

        agent.Enabled = !agent.Enabled;
        agent.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return agent.Enabled;
    }
}