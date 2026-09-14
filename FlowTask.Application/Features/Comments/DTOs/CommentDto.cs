namespace FlowTask.Application.Features.Comments.DTOs;

public record CommentDto(
    int      Id,
    int      TaskId,
    int      UserId,
    string   UserName,
    string?  UserAvatarUrl,
    string   Text,
    bool     Resolved,
    List<ReactionDto> Reactions,
    DateTime CreatedAt
);

public record ReactionDto(string Emoji, int Count, bool ReactedByMe);