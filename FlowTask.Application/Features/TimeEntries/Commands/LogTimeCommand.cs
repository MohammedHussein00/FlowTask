namespace FlowTask.Application.Features.TimeEntries.Commands;

using MediatR;
using FlowTask.Application.Features.TimeEntries.DTOs;

public record LogTimeCommand(
    int      TaskId,
    DateTime StartTime,
    DateTime EndTime,
    string?  Description = null
) : IRequest<TimeEntryDto>;

public record DeleteTimeEntryCommand(int Id) : IRequest<bool>;