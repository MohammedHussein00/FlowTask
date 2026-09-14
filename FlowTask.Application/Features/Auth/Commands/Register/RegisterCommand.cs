namespace FlowTask.Application.Features.Auth.Commands.Register;

using FlowTask.Application.Features.Auth.DTOs;
using FlowTask.Shared.Results;
using MediatR;

public record RegisterCommand(string FullName, string Email, string Password)
    : IRequest<Result<AuthResultDto>>;