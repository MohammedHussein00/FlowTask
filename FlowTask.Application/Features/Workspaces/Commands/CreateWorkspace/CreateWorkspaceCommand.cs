namespace FlowTask.Application.Features.Workspaces.Commands.CreateWorkspace;

using MediatR;
using FlowTask.Application.Features.Workspaces.DTOs;

public record CreateWorkspaceCommand(
    string  Name,
    string? AvatarUrl
) : IRequest<WorkspaceDto>;