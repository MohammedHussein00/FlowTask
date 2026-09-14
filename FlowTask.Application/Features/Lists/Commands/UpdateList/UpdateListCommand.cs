namespace FlowTask.Application.Features.Lists.Commands.UpdateList;

using MediatR;
using FlowTask.Application.Features.Lists.DTOs;

public record UpdateListCommand(
    int      Id,
    string   Name,
    string?  Content   = null,
    string   Priority  = "normal",
    DateTime? StartDate = null,
    DateTime? DueDate   = null
) : IRequest<ListDto>;