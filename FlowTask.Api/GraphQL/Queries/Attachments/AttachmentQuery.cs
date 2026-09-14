namespace FlowTask.Api.GraphQL.Queries.Attachments;

using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Interfaces;
using FlowTask.Application.Features.Attachments.DTOs;

[ExtendObjectType("Query")]
public class AttachmentQuery
{
    [Authorize]
    public async Task<List<AttachmentDto>> GetAttachmentsByTask(
        int taskId,
        [Service] IApplicationDbContext db,
        CancellationToken ct)
        => await db.Attachments
            .Where(a => a.TaskId == taskId)
            .Select(a => new AttachmentDto(
                a.Id,
                a.TaskId,
                a.Task.Name,
                a.UploaderId,
                a.Uploader.FullName ?? a.Uploader.UserName,
                a.FileName,
                a.FileSize,
                a.FileUrl,
                System.IO.Path.GetExtension(a.FileName),
                a.CreatedAt))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);
}