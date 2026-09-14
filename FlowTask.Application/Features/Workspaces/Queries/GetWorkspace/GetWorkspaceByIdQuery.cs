namespace FlowTask.Application.Features.Workspaces.Queries.GetWorkspace;

using MediatR;
using FlowTask.Application.Features.Workspaces.DTOs;

public record GetWorkspaceByIdQuery(int Id) : IRequest<WorkspaceDto>;