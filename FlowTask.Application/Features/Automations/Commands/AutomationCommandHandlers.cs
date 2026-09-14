namespace FlowTask.Application.Features.Automations.Commands;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Automations.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Automations;

public class CreateAutomationCommandHandler
    : IRequestHandler<CreateAutomationCommand, AutomationDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CreateAutomationCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<AutomationDto> Handle(
        CreateAutomationCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        int? userId = _currentUser.UserId;

        var automation = new Automation
        {
            WorkspaceId = request.WorkspaceId,
            Name = request.Name,
            Description = request.Description,
            TriggerType = request.TriggerType,
            SpaceId = request.SpaceId,
            ListId = request.ListId,
            ConditionLogic = request.ConditionLogic,
            IsActive = true,
            CreatedBy = userId
        };

        _db.Automations.Add(automation);
        await _db.SaveChangesAsync(ct);

        return await ProjectToDto(automation.Id, ct);
    }

    private async Task<AutomationDto> ProjectToDto(int id, CancellationToken ct)
        => await _db.Automations
            .Where(a => a.Id == id)
            .Select(a => new AutomationDto(
                a.Id,
                a.WorkspaceId,
                a.SpaceId,
                a.ListId,
                a.Name,
                a.Description,
                a.IsActive,
                a.TriggerType,
                a.ConditionLogic,
                a.CreatedBy,
                a.CreatedByUser.FullName ?? a.CreatedByUser.UserName,
                a.Triggers.Select(t => new TriggerDto(
                    t.Id,
                    t.TriggerEvent,
                    t.EntityType,
                    t.Conditions,
                    t.IsActive)).ToList(),
                a.Actions.Select(ac => new ActionDto(
                    ac.Id,
                    ac.ActionType,
                    ac.ActionConfig,
                    ac.OrderIndex,
                    ac.IsActive)).ToList(),
                a.CreatedAt))
            .FirstAsync(ct);
}

public class ToggleAutomationCommandHandler
    : IRequestHandler<ToggleAutomationCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public ToggleAutomationCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(ToggleAutomationCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var automation = await _db.Automations
            .FirstOrDefaultAsync(a => a.Id == request.AutomationId, ct);

        if (automation == null)
        {
            throw new NotFoundException(
                localizationKey: "AutomationNotFound",
                args: [request.AutomationId],
                devMessage: $"Automation with id '{request.AutomationId}' was not found.");
        }

        automation.IsActive = !automation.IsActive;
        automation.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return automation.IsActive;
    }
}

public class AddAutomationTriggerCommandHandler
    : IRequestHandler<AddAutomationTriggerCommand, TriggerDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public AddAutomationTriggerCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<TriggerDto> Handle(
        AddAutomationTriggerCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var automationExists = await _db.Automations
            .AnyAsync(a => a.Id == request.AutomationId, ct);

        if (!automationExists)
        {
            throw new NotFoundException(
                localizationKey: "AutomationNotFound",
                args: [request.AutomationId],
                devMessage: $"Automation with id '{request.AutomationId}' was not found.");
        }

        var trigger = new AutomationTrigger
        {
            AutomationId = request.AutomationId,
            TriggerEvent = request.TriggerEvent,
            EntityType = request.EntityType,
            Conditions = request.Conditions,
            IsActive = true
        };

        _db.AutomationTriggers.Add(trigger);
        await _db.SaveChangesAsync(ct);

        return new TriggerDto(
            trigger.Id,
            trigger.TriggerEvent,
            trigger.EntityType,
            trigger.Conditions,
            trigger.IsActive);
    }
}

public class AddAutomationActionCommandHandler
    : IRequestHandler<AddAutomationActionCommand, ActionDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public AddAutomationActionCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<ActionDto> Handle(
        AddAutomationActionCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var automationExists = await _db.Automations
            .AnyAsync(a => a.Id == request.AutomationId, ct);

        if (!automationExists)
        {
            throw new NotFoundException(
                localizationKey: "AutomationNotFound",
                args: [request.AutomationId],
                devMessage: $"Automation with id '{request.AutomationId}' was not found.");
        }

        var action = new AutomationAction
        {
            AutomationId = request.AutomationId,
            ActionType = request.ActionType,
            ActionConfig = request.ActionConfig,
            OrderIndex = request.OrderIndex,
            IsActive = true
        };

        _db.AutomationActions.Add(action);
        await _db.SaveChangesAsync(ct);

        return new ActionDto(
            action.Id,
            action.ActionType,
            action.ActionConfig,
            action.OrderIndex,
            action.IsActive);
    }
}

public class DeleteAutomationCommandHandler
    : IRequestHandler<DeleteAutomationCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public DeleteAutomationCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(DeleteAutomationCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var userId = _currentUser.UserId;
        var automation = await _db.Automations
            .FirstOrDefaultAsync(a => a.Id == request.AutomationId, ct);

        if (automation == null)
        {
            throw new NotFoundException(
                localizationKey: "AutomationNotFound",
                args: [request.AutomationId],
                devMessage: $"Automation with id '{request.AutomationId}' was not found.");
        }

        if (automation.CreatedBy != userId)
        {
            throw new UnauthorizedException(
                localizationKey: "OnlyCreatorCanDeleteAutomation",
                devMessage: "Only the creator can delete this automation.");
        }

        _db.Automations.Remove(automation);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}