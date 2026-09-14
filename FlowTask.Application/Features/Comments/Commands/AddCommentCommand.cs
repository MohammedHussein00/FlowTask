namespace FlowTask.Application.Features.Comments.Commands;

using MediatR;
using FlowTask.Application.Features.Comments.DTOs;

public record AddCommentCommand(int TaskId, string Text) : IRequest<CommentDto>;
public record EditCommentCommand(int CommentId, string Text) : IRequest<CommentDto>;
public record DeleteCommentCommand(int CommentId) : IRequest<bool>;
public record ResolveCommentCommand(int CommentId, bool Resolved) : IRequest<CommentDto>;
public record ReactToCommentCommand(int CommentId, string Emoji) : IRequest<bool>;