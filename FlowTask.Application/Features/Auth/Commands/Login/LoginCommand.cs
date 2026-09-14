namespace FlowTask.Application.Features.Auth.Commands.Login;

using MediatR;
using FlowTask.Application.Features.Auth.DTOs;
using FlowTask.Shared.Results;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResultDto>>;