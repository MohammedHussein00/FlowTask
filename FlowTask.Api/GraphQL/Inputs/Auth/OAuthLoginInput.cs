namespace FlowTask.Api.GraphQL.Inputs.Auth;

using FlowTask.Application.Features.Auth.OAuth;

public sealed class OAuthLoginInput
{
    public OAuthProvider Provider { get; init; }
    public string Code { get; init; } = default!;
}