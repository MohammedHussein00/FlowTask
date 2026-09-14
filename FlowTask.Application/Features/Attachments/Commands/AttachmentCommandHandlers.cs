namespace FlowTask.Application.Features.Attachments.Commands;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Attachments.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Collaboration;

public class AddAttachmentCommandHandler
    : IRequestHandler<AddAttachmentCommand, AttachmentDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public AddAttachmentCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<AttachmentDto> Handle(
        AddAttachmentCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var taskExists = await _db.Tasks.AnyAsync(t => t.Id == request.TaskId, ct);
        if (!taskExists)
            throw new NotFoundException(
                localizationKey: "TaskNotFound",
                args: [request.TaskId],
                devMessage: $"Task with id '{request.TaskId}' was not found.");

        var attachment = new Attachment
        {
            TaskId = request.TaskId,
            UploaderId = userId,
            FileName = request.FileName,
            FileUrl = request.FileUrl,
            FileSize = request.FileSize
        };

        _db.Attachments.Add(attachment);
        await _db.SaveChangesAsync(ct);

        return await _db.Attachments
            .Where(a => a.Id == attachment.Id)
            .Select(a => new AttachmentDto(
                a.Id, a.TaskId, a.Task.Name,
                a.UploaderId,
                a.Uploader.FullName ?? a.Uploader.UserName!,
                a.FileName, a.FileSize, a.FileUrl,
                Path.GetExtension(a.FileName),
                a.CreatedAt))
            .FirstAsync(ct);
    }
}

public class DeleteAttachmentCommandHandler
    : IRequestHandler<DeleteAttachmentCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public DeleteAttachmentCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(DeleteAttachmentCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();
        var attachment = await _db.Attachments
            .FirstOrDefaultAsync(a => a.Id == request.AttachmentId, ct)
            ?? throw new NotFoundException(
                localizationKey: "AttachmentNotFound",
                args: [request.AttachmentId],
                devMessage: $"Attachment with id '{request.AttachmentId}' was not found.");

        if (attachment.UploaderId != userId)
            throw new UnauthorizedException(
                localizationKey: "OnlyUploaderCanDeleteAttachment",
                devMessage: "Only the uploader can delete this attachment.");

        _db.Attachments.Remove(attachment);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}