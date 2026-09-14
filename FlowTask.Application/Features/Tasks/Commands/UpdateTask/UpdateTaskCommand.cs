namespace FlowTask.Application.Features.Tasks.Commands.UpdateTask;

using MediatR;
using FlowTask.Application.Features.Tasks.DTOs;

public record UpdateTaskCommand(
    int      Id,
    string   Name,
    string?  Description  = null,
    string   Priority     = "normal",
    int?     StatusId     = null,
    DateTime? StartDate   = null,
    DateTime? DueDate     = null,
    int?     TimeEstimate = null
) : IRequest<TaskDto>;