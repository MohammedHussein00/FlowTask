namespace FlowTask.Application.Features.Tasks.Commands.CreateTask;

using MediatR;
using FlowTask.Application.Features.Tasks.DTOs;

public record CreateTaskCommand(
    int      ListId,
    string   Name,
    string?  Description  = null,
    string   Priority     = "normal",
    int?     StatusId     = null,
    int?     ParentId     = null,
    DateTime? StartDate   = null,
    DateTime? DueDate     = null,
    int?     TimeEstimate = null,
    List<int>? AssigneeIds = null
) : IRequest<TaskDto>;