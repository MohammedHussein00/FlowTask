namespace FlowTask.Api.GraphQL.Mutations.Goals;

using FlowTask.Application.Features.Goals.Commands;
using FlowTask.Application.Features.Goals.DTOs;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Mutation")]
public class GoalMutation
{
    [Authorize]
    public async Task<GoalDto> CreateGoal(
        int workspaceId, string name,
        string? description, DateTime? dueDate, string color,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateGoalCommand(
            workspaceId, name, description, dueDate, color), ct);

    [Authorize]
    public async Task<KeyResultDto> AddKeyResult(
        int goalId, string name, string resultType, decimal? targetValue,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new AddKeyResultCommand(
            goalId, name, resultType, targetValue), ct);

    [Authorize]
    public async Task<KeyResultDto> UpdateKeyResultProgress(
        int keyResultId, decimal currentValue,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new UpdateKeyResultProgressCommand(
            keyResultId, currentValue), ct);
}