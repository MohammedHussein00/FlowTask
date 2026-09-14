namespace FlowTask.Api.GraphQL.Types.Workspaces;

using FlowTask.Application.Features.Workspaces.DTOs;

public class WorkspaceType
{
    public int      Id          { get; init; }
    public string   Name        { get; init; } = default!;
    public string?  AvatarUrl   { get; init; }
    public int      OwnerId     { get; init; }
    public string   OwnerName   { get; init; } = default!;
    public int      MemberCount { get; init; }
    public DateTime CreatedAt   { get; init; }

    public static WorkspaceType FromDto(WorkspaceDto dto) => new()
    {
        Id          = dto.Id,
        Name        = dto.Name,
        AvatarUrl   = dto.AvatarUrl,
        OwnerId     = dto.OwnerId,
        OwnerName   = dto.OwnerName,
        MemberCount = dto.MemberCount,
        CreatedAt   = dto.CreatedAt
    };
}