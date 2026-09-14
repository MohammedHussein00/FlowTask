namespace FlowTask.Application.Features.Workspaces.Queries.GetMyWorkspaces;

using MediatR;
using FlowTask.Application.Features.Workspaces.DTOs;

/// <summary>
/// Returns every workspace the current user is a member of (owner or otherwise),
/// most recently created first. Backs the top-left workspace switcher on the frontend.
/// </summary>
public record GetMyWorkspacesQuery : IRequest<List<WorkspaceDto>>;