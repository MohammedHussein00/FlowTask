namespace FlowTask.Api.GraphQL.Mutations.Comments;

using FlowTask.Application.Features.Comments.Commands;
using FlowTask.Application.Features.Comments.DTOs;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Mutation")]
public class CommentMutation
{
    [Authorize]
    public async Task<CommentDto> AddComment(
        int taskId, string text,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new AddCommentCommand(taskId, text), ct);

    [Authorize]
    public async Task<CommentDto> EditComment(
        int commentId, string text,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new EditCommentCommand(commentId, text), ct);

    [Authorize]
    public async Task<bool> DeleteComment(
        int commentId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteCommentCommand(commentId), ct);

    [Authorize]
    public async Task<CommentDto> ResolveComment(
        int commentId, bool resolved,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ResolveCommentCommand(commentId, resolved), ct);

    [Authorize]
    public async Task<bool> ReactToComment(
        int commentId, string emoji,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ReactToCommentCommand(commentId, emoji), ct);
}