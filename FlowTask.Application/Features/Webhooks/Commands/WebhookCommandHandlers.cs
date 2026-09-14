namespace FlowTask.Application.Features.Webhooks.Commands;

using MediatR;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Webhooks.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.SystemOps;

public class CreateWebhookCommandHandler
    : IRequestHandler<CreateWebhookCommand, WebhookDto>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CreateWebhookCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<WebhookDto> Handle(
        CreateWebhookCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
        {
            throw new ValidationException(
        new Dictionary<string, IReadOnlyList<(string, object[])>>
        {
            ["Url"] = new List<(string, object[])> { ("UrlMustBeAbsolute", Array.Empty<object>()) }
        });
        }

        var webhook = new Webhook
        {
            WorkspaceId = request.WorkspaceId,
            Url = request.Url,
            Events = request.Events,
            Secret = Guid.NewGuid().ToString("N"),
            IsActive = true
        };

        _db.Webhooks.Add(webhook);
        await _db.SaveChangesAsync(ct);

        return new WebhookDto(
            webhook.Id,
            webhook.WorkspaceId,
            webhook.Url,
            webhook.Events,
            webhook.IsActive,
            DateTime.UtcNow);
    }
}

public class DeleteWebhookCommandHandler
    : IRequestHandler<DeleteWebhookCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public DeleteWebhookCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(DeleteWebhookCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var webhook = await _db.Webhooks
            .FirstOrDefaultAsync(w => w.Id == request.WebhookId, ct);

        if (webhook == null)
        {
            throw new
                     NotFoundException(localizationKey: "WebhookNotFound", args: [request.WebhookId], devMessage: $"Webhook with id '{request.WebhookId}' was not found.")
;
        }

        _db.Webhooks.Remove(webhook);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}

public class ToggleWebhookCommandHandler
    : IRequestHandler<ToggleWebhookCommand, bool>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public ToggleWebhookCommandHandler(
        IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(ToggleWebhookCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId == null)
        {
            throw new UnauthorizedException();
        }

        var webhook = await _db.Webhooks
            .FirstOrDefaultAsync(w => w.Id == request.WebhookId, ct);

        if (webhook == null)
        {
            throw new 
                NotFoundException(localizationKey: "WebhookNotFound", args: [request.WebhookId], devMessage: $"Webhook with id '{request.WebhookId}' was not found.");
        }

        webhook.IsActive = !webhook.IsActive;
        await _db.SaveChangesAsync(ct);
        return webhook.IsActive;
    }
}