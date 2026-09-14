namespace FlowTask.Application.Features.Workspaces.Commands.UpdateWorkspace;

using MediatR;
using FlowTask.Application.Features.Workspaces.DTOs;

public record UpdateWorkspaceCommand(
    int     Id,
    string  Name,
    string? AvatarUrl
) : IRequest<WorkspaceDto>;