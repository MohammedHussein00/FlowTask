namespace FlowTask.Api.GraphQL.Mutations.Attachments;

using FlowTask.Application.Features.Attachments.Commands;
using FlowTask.Application.Features.Attachments.DTOs;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using MediatR;

[ExtendObjectType("Mutation")]
public class AttachmentMutation
{
    [Authorize]
    public async Task<AttachmentDto> AddAttachment(
        int taskId, string fileName, string fileUrl, int? fileSize,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(
            new AddAttachmentCommand(taskId, fileName, fileUrl, fileSize), ct);

    [Authorize]
    public async Task<bool> DeleteAttachment(
        int attachmentId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteAttachmentCommand(attachmentId), ct);
}