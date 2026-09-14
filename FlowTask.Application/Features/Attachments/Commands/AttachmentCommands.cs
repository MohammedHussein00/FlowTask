namespace FlowTask.Application.Features.Attachments.Commands;

using MediatR;
using FlowTask.Application.Features.Attachments.DTOs;

public record AddAttachmentCommand(
    int     TaskId,
    string  FileName,
    string  FileUrl,
    int?    FileSize = null
) : IRequest<AttachmentDto>;

public record DeleteAttachmentCommand(int AttachmentId) : IRequest<bool>;