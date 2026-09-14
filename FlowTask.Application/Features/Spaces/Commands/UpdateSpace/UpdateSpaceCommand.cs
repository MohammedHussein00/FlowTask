namespace FlowTask.Application.Features.Spaces.Commands.UpdateSpace;

using MediatR;
using FlowTask.Application.Features.Spaces.DTOs;

public record UpdateSpaceCommand(
    int     Id,
    string  Name,
    bool    IsPrivate = false,
    string  Color     = "#5B9BD5",
    string? AvatarUrl = null
) : IRequest<SpaceDto>;