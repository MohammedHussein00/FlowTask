namespace FlowTask.Application.Features.Auth.Commands.OAuthLogin;

using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Auth.DTOs;
using FlowTask.Application.Features.Auth.OAuth;
using MediatR;
using Microsoft.Extensions.Logging;

/// <summary>
/// NOTE: do NOT register a FluentValidation validator for this command.
/// ValidationBehavior requires TResponse to expose a static Fail(ResultError)
/// method; AuthResultDto doesn't have one (unlike Result/Result&lt;T&gt;), so a
/// validator here would throw InvalidOperationException at runtime. Validate
/// inline if you ever need to (e.g. reject an empty code).
/// </summary>
public class OAuthLoginCommandHandler : IRequestHandler<OAuthLoginCommand, AuthResultDto>
{
    private readonly IEnumerable<IOAuthProviderService> _providers;
    private readonly IOAuthUserProvisioningService _provisioning;
    private readonly ILogger<OAuthLoginCommandHandler> _logger;

    public OAuthLoginCommandHandler(
        IEnumerable<IOAuthProviderService> providers,
        IOAuthUserProvisioningService provisioning,
        ILogger<OAuthLoginCommandHandler> logger)
    {
        _providers = providers;
        _provisioning = provisioning;
        _logger = logger;
    }

    public async Task<AuthResultDto> Handle(OAuthLoginCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
            throw new BadRequestException("OAuthCodeRequired", "Auth");

        var provider = _providers.FirstOrDefault(p => p.Provider == request.Provider)
            ?? throw new BadRequestException("UnsupportedProvider", "Auth");

        OAuthUserInfo info;
        try
        {
            info = await provider.ExchangeCodeAsync(request.Code, ct);
        }
        catch (OAuthExchangeException ex)
        {
            // This is the actual reason — check your API console/logs for this
            // line. Client-facing message stays generic (OAuthExchangeFailed)
            // on purpose so we don't leak provider internals to the browser.
            _logger.LogWarning(ex,
                "OAuth exchange failed for provider {Provider}. ErrorCode={ErrorCode} Message={Message}",
                request.Provider, ex.ErrorCode, ex.Message);

            throw new UnauthenticatedException("OAuthExchangeFailed", "Auth");
        }

        if (!info.EmailVerified)
            throw new ForbiddenException("OAuthEmailNotVerified", "Auth");

        return await _provisioning.GetOrCreateAndSignInAsync(request.Provider, info, ct);
    }
}