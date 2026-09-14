namespace FlowTask.Application.Features.Spaces.Queries;

using MediatR;
using FlowTask.Application.Features.Spaces.DTOs;

public record GetSpacesByWorkspaceQuery(int WorkspaceId) : IRequest<List<SpaceDto>>;