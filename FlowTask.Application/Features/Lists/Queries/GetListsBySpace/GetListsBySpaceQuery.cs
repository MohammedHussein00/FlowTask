namespace FlowTask.Application.Features.Lists.Queries;

using MediatR;
using FlowTask.Application.Features.Lists.DTOs;

public record GetListsBySpaceQuery(int SpaceId) : IRequest<List<ListDto>>;