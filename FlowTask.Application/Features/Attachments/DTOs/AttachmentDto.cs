namespace FlowTask.Application.Features.Attachments.DTOs;

public record AttachmentDto(
    int      Id,
    int      TaskId,
    string   TaskName,
    int      UploaderId,
    string   UploaderName,
    string   FileName,
    int?     FileSize,
    string   FileUrl,
    string   FileExtension,
    DateTime CreatedAt
);