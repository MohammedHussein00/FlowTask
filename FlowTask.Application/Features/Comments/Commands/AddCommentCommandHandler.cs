namespace FlowTask.Application.Features.Comments.Commands;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Comments.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Collaboration;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public AddCommentCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<CommentDto> Handle(AddCommentCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var taskExists = await _db.Tasks.AnyAsync(t => t.Id == request.TaskId, ct);
        if (!taskExists)
            throw new NotFoundException(
                localizationKey: "TaskNotFound",
                args: [request.TaskId],
                devMessage: $"Task with id '{request.TaskId}' was not found.");

        var comment = new Comment
        {
            TaskId = request.TaskId,
            UserId = userId,
            Text = request.Text
        };

        _db.Comments.Add(comment);
        await _db.SaveChangesAsync(ct);

        return await ProjectToDto(comment.Id, userId, ct);
    }

    private async Task<CommentDto> ProjectToDto(int commentId, int currentUserId, CancellationToken ct)
    {
        return await _db.Comments
            .Where(c => c.Id == commentId)
            .Select(c => new CommentDto(
                c.Id, c.TaskId, c.UserId,
                c.User.FullName ?? c.User.UserName!,
                c.User.AvatarUrl,
                c.Text,
                c.Resolved,
                c.Reactions
                    .GroupBy(r => r.Emoji)
                    .Select(g => new ReactionDto(
                        g.Key, g.Count(),
                        g.Any(r => r.UserId == currentUserId)))
                    .ToList(),
                c.CreatedAt))
            .FirstAsync(ct);
    }
}

public class EditCommentCommandHandler : IRequestHandler<EditCommentCommand, CommentDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public EditCommentCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<CommentDto> Handle(EditCommentCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();
        var comment = await _db.Comments.FindAsync([request.CommentId], ct)
            ?? throw new NotFoundException(
                localizationKey: "CommentNotFound",
                args: [request.CommentId],
                devMessage: $"Comment with id '{request.CommentId}' was not found.");

        if (comment.UserId != userId)
            throw new UnauthorizedException(
                localizationKey: "OnlyOwnerCanEditComment",
                devMessage: "You can only edit your own comments.");

        comment.Text = request.Text;
        await _db.SaveChangesAsync(ct);

        return await _db.Comments
            .Where(c => c.Id == comment.Id)
            .Select(c => new CommentDto(
                c.Id, c.TaskId, c.UserId,
                c.User.FullName ?? c.User.UserName!,
                c.User.AvatarUrl, c.Text, c.Resolved,
                c.Reactions.GroupBy(r => r.Emoji)
                    .Select(g => new ReactionDto(g.Key, g.Count(), g.Any(r => r.UserId == userId)))
                    .ToList(),
                c.CreatedAt))
            .FirstAsync(ct);
    }
}

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public DeleteCommentCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();
        var comment = await _db.Comments.FindAsync([request.CommentId], ct)
            ?? throw new NotFoundException(
                localizationKey: "CommentNotFound",
                args: [request.CommentId],
                devMessage: $"Comment with id '{request.CommentId}' was not found.");

        if (comment.UserId != userId)
            throw new UnauthorizedException(
                localizationKey: "OnlyOwnerCanDeleteComment",
                devMessage: "You can only delete your own comments.");

        _db.Comments.Remove(comment);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}

public class ResolveCommentCommandHandler : IRequestHandler<ResolveCommentCommand, CommentDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public ResolveCommentCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<CommentDto> Handle(ResolveCommentCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();
        var comment = await _db.Comments.FindAsync([request.CommentId], ct)
            ?? throw new NotFoundException(
                localizationKey: "CommentNotFound",
                args: [request.CommentId],
                devMessage: $"Comment with id '{request.CommentId}' was not found.");

        comment.Resolved = request.Resolved;
        await _db.SaveChangesAsync(ct);

        return await _db.Comments
            .Where(c => c.Id == comment.Id)
            .Select(c => new CommentDto(
                c.Id, c.TaskId, c.UserId,
                c.User.FullName ?? c.User.UserName!,
                c.User.AvatarUrl, c.Text, c.Resolved,
                c.Reactions.GroupBy(r => r.Emoji)
                    .Select(g => new ReactionDto(g.Key, g.Count(), g.Any(r => r.UserId == userId)))
                    .ToList(),
                c.CreatedAt))
            .FirstAsync(ct);
    }
}

public class ReactToCommentCommandHandler : IRequestHandler<ReactToCommentCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public ReactToCommentCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(ReactToCommentCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var existing = await _db.CommentReactions
            .FirstOrDefaultAsync(r =>
                r.CommentId == request.CommentId &&
                r.UserId == userId &&
                r.Emoji == request.Emoji, ct);

        if (existing is not null)
            _db.CommentReactions.Remove(existing); // toggle off
        else
            _db.CommentReactions.Add(new CommentReaction
            {
                CommentId = request.CommentId,
                UserId = userId,
                Emoji = request.Emoji
            });

        await _db.SaveChangesAsync(ct);
        return true;
    }
}