namespace FlowTask.Api.GraphQL.Queries.Invitations;

using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Interfaces;
using FlowTask.Application.Features.Invitations.DTOs;

[ExtendObjectType("Query")]
public class InvitationQuery
{
    [Authorize]
    public async Task<List<InvitationDto>> GetWorkspaceInvitations(
        int workspaceId,
        [Service] IApplicationDbContext db,
        CancellationToken ct)
        => await db.Invitations
            .Where(i => i.WorkspaceId == workspaceId)
            .Select(i => new InvitationDto(
                i.Id, i.WorkspaceId,
                i.Workspace.Name,
                i.Email, i.InvitedBy,
                i.InvitedByUser.FullName ?? i.InvitedByUser.UserName!,
                i.Role, i.Status,
                i.ExpiresAt, i.CreatedAt))
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(ct);
}