namespace FlowTask.Application.Features.Tasks.Commands.DeleteTask;

using MediatR;

public record DeleteTaskCommand(int Id) : IRequest<bool>;