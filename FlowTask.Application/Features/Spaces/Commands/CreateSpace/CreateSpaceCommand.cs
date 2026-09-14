namespace FlowTask.Application.Features.Spaces.Commands.CreateSpace;

using MediatR;
using FlowTask.Application.Features.Spaces.DTOs;

public record CreateSpaceCommand(
    int     WorkspaceId,
    string  Name,
    bool    IsPrivate  = false,
    string  Color      = "#5B9BD5",
    string? AvatarUrl  = null
) : IRequest<SpaceDto>;