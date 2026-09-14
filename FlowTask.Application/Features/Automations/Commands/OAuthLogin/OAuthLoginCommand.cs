// FlowTask.Application/Features/Auth/Commands/OAuthLogin/OAuthLoginCommand.cs
namespace FlowTask.Application.Features.Auth.Commands.OAuthLogin;

using FlowTask.Application.Features.Auth.DTOs;
using FlowTask.Application.Features.Auth.OAuth;
using MediatR;

public record OAuthLoginCommand(OAuthProvider Provider, string Code) : IRequest<AuthResultDto>;