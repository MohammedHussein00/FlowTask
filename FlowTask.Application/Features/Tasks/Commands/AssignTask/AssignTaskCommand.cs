namespace FlowTask.Application.Features.Tasks.Commands.AssignTask;

using MediatR;

public record AssignTaskCommand(int TaskId, int UserId) : IRequest<bool>;
public record UnassignTaskCommand(int TaskId, int UserId) : IRequest<bool>;