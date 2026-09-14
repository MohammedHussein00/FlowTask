namespace FlowTask.Application.Features.Goals.Commands;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Goals.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Productivity;

public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, GoalDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CreateGoalCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<GoalDto> Handle(CreateGoalCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        int? userId = _currentUser.UserId;

        var goal = new Goal
        {
            WorkspaceId = request.WorkspaceId,
            Name = request.Name,
            Description = request.Description,
            DueDate = request.DueDate,
            Color = request.Color,
            OwnerId = userId
        };

        _db.Goals.Add(goal);
        await _db.SaveChangesAsync(ct);

        return await _db.Goals
            .Where(g => g.Id == goal.Id)
            .Select(g => new GoalDto(
                g.Id,
                g.WorkspaceId,
                g.Name,
                g.Description,
                g.OwnerId,
                g.Owner.FullName ?? g.Owner.UserName,
                g.DueDate,
                g.Color,
                g.KeyResults.Count == 0 ? 0 :
                    g.KeyResults.Average(kr =>
                        kr.TargetValue == null || kr.TargetValue == 0 ? 0 :
                        (double)(kr.CurrentValue / kr.TargetValue.Value) * 100),
                g.KeyResults.Select(kr => new KeyResultDto(
                    kr.Id,
                    kr.GoalId,
                    kr.Name,
                    kr.ResultType,
                    kr.TargetValue,
                    kr.CurrentValue,
                    kr.OwnerId,
                    kr.Owner.FullName ?? kr.Owner.UserName,
                    kr.TargetValue == null || kr.TargetValue == 0 ? 0 :
                        (double)(kr.CurrentValue / kr.TargetValue.Value) * 100)).ToList(),
                g.CreatedAt))
            .FirstAsync(ct);
    }
}

public class AddKeyResultCommandHandler : IRequestHandler<AddKeyResultCommand, KeyResultDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public AddKeyResultCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<KeyResultDto> Handle(AddKeyResultCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        int? userId = _currentUser.UserId;

        var goalExists = await _db.Goals.AnyAsync(g => g.Id == request.GoalId, ct);

        if (!goalExists)
        {
            throw new NotFoundException(
                localizationKey: "GoalNotFound",
                args: [request.GoalId],
                devMessage: $"Goal with id '{request.GoalId}' was not found.");
        }

        var kr = new KeyResult
        {
            GoalId = request.GoalId,
            Name = request.Name,
            ResultType = request.ResultType,
            TargetValue = request.TargetValue,
            OwnerId = userId
        };

        _db.KeyResults.Add(kr);
        await _db.SaveChangesAsync(ct);

        return await _db.KeyResults
            .Where(k => k.Id == kr.Id)
            .Select(k => new KeyResultDto(
                k.Id,
                k.GoalId,
                k.Name,
                k.ResultType,
                k.TargetValue,
                k.CurrentValue,
                k.OwnerId,
                k.Owner.FullName ?? k.Owner.UserName,
                k.TargetValue == null || k.TargetValue == 0 ? 0 :
                    (double)(k.CurrentValue / k.TargetValue.Value) * 100))
            .FirstAsync(ct);
    }
}

public class UpdateKeyResultProgressCommandHandler
    : IRequestHandler<UpdateKeyResultProgressCommand, KeyResultDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public UpdateKeyResultProgressCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<KeyResultDto> Handle(UpdateKeyResultProgressCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var kr = await _db.KeyResults.FindAsync([request.KeyResultId], ct);

        if (kr == null)
        {
            throw new NotFoundException(
                localizationKey: "KeyResultNotFound",
                args: [request.KeyResultId],
                devMessage: $"KeyResult with id '{request.KeyResultId}' was not found.");
        }

        kr.CurrentValue = request.CurrentValue;
        await _db.SaveChangesAsync(ct);

        return await _db.KeyResults
            .Where(k => k.Id == kr.Id)
            .Select(k => new KeyResultDto(
                k.Id,
                k.GoalId,
                k.Name,
                k.ResultType,
                k.TargetValue,
                k.CurrentValue,
                k.OwnerId,
                k.Owner.FullName ?? k.Owner.UserName,
                k.TargetValue == null || k.TargetValue == 0 ? 0 :
                    (double)(k.CurrentValue / k.TargetValue.Value) * 100))
            .FirstAsync(ct);
    }
}