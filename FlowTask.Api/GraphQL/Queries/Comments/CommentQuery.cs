namespace FlowTask.Api.GraphQL.Queries.Comments;

using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Interfaces;
using FlowTask.Application.Features.Comments.DTOs;

[ExtendObjectType("Query")]
public class CommentQuery
{
    [Authorize]
    public async Task<List<CommentDto>> GetCommentsByTask(
        int taskId,
        [Service] IApplicationDbContext db,
        [Service] ICurrentUserService currentUser,
        CancellationToken ct)
    {
        var userId = currentUser.UserId ?? 0;

        return await db.Comments
            .Where(c => c.TaskId == taskId)
            .Select(c => new CommentDto(
                c.Id, c.TaskId, c.UserId,
                c.User.FullName ?? c.User.UserName!,
                c.User.AvatarUrl, c.Text, c.Resolved,
                c.Reactions.GroupBy(r => r.Emoji)
                    .Select(g => new ReactionDto(g.Key, g.Count(), g.Any(r => r.UserId == userId)))
                    .ToList(),
                c.CreatedAt))
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(ct);
    }
}