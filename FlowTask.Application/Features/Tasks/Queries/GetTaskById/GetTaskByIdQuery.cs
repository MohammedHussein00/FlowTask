
namespace FlowTask.Application.Features.Tasks.Queries.GetTaskById;

using MediatR;
using FlowTask.Application.Features.Tasks.DTOs;

public record GetTaskByIdQuery(int Id) : IRequest<TaskDto>;