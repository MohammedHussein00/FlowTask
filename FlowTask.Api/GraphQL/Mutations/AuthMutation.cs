namespace FlowTask.Api.GraphQL.Mutations;

using HotChocolate;
using HotChocolate.Types;
using MediatR;
using FlowTask.Application.Features.Auth.Commands.Login;
using FlowTask.Application.Features.Auth.Commands.Register;
using FlowTask.Application.Features.Auth.Commands.OAuthLogin;
using FlowTask.Api.GraphQL.Inputs.Auth;
using FlowTask.Api.GraphQL.Types;
using FlowTask.Api.Extensions;
using FlowTask.Shared.Localization;

[ExtendObjectType("Mutation")]
public class AuthMutation
{
    public async Task<Payload<AuthResultType>> Register(
        RegisterInput input,
        [Service] IMediator mediator,
        [Service] ILocalizationService loc,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new RegisterCommand(input.FullName, input.Email, input.Password), ct);

        return result.ToPayload(loc, AuthResultType.FromDto);
    }

    public async Task<Payload<AuthResultType>> Login(
        LoginInput input,
        [Service] IMediator mediator,
        [Service] ILocalizationService loc,
        CancellationToken ct)
    {
        var result = await mediator.Send(new LoginCommand(input.Email, input.Password), ct);

        return result.ToPayload(loc, AuthResultType.FromDto);
    }

    /// <summary>
    /// Handles both "Continue with Google" and "Continue with GitHub". Unlike
    /// Register/Login, failures here are surfaced as a top-level GraphQL error
    /// (via ErrorFilter) rather than inside Payload.error — the frontend's
    /// handleBackendError already reads `graphQLErrors[0].extensions` first,
    /// so oauthError gets set the same way a network/provider error would.
    /// </summary>
    public async Task<Payload<AuthResultType>> OAuthLogin(
        OAuthLoginInput input,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        var dto = await mediator.Send(new OAuthLoginCommand(input.Provider, input.Code), ct);
        return Payload<AuthResultType>.Ok(AuthResultType.FromDto(dto));
    }
}