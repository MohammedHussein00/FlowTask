namespace FlowTask.Application.Features.Lists.Commands.CreateList;

using MediatR;
using FlowTask.Application.Features.Lists.DTOs;

public record CreateListCommand(
    int      SpaceId,
    string   Name,
    int?     FolderId  = null,
    string?  Content   = null,
    string   Priority  = "normal",
    DateTime? StartDate = null,
    DateTime? DueDate   = null
) : IRequest<ListDto>;