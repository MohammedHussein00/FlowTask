namespace FlowTask.Api.GraphQL.Queries.Automations;

using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Interfaces;
using FlowTask.Application.Features.Automations.DTOs;

[ExtendObjectType("Query")]
public class AutomationQuery
{
    [Authorize]
    public async Task<List<AutomationDto>> GetAutomationsByWorkspace(
        int workspaceId,
        [Service] IApplicationDbContext db,
        CancellationToken ct)
        => await db.Automations
            .Where(a => a.WorkspaceId == workspaceId)
            .Select(a => new AutomationDto(
                a.Id, a.WorkspaceId, a.SpaceId, a.ListId,
                a.Name, a.Description, a.IsActive, a.TriggerType,
                a.ConditionLogic, a.CreatedBy,
                a.CreatedByUser.FullName ?? a.CreatedByUser.UserName!,
                a.Triggers.Select(t => new TriggerDto(
                    t.Id, t.TriggerEvent, t.EntityType, t.Conditions, t.IsActive)).ToList(),
                a.Actions.Select(ac => new ActionDto(
                    ac.Id, ac.ActionType, ac.ActionConfig, ac.OrderIndex, ac.IsActive)).ToList(),
                a.CreatedAt))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);
}