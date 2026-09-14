namespace FlowTask.Application.Features.Tasks.Queries.GetTasksByList;

using MediatR;
using FlowTask.Application.Features.Tasks.DTOs;

public record GetTasksByListQuery(int ListId) : IRequest<List<TaskDto>>;