namespace FlowTask.Application.Features.Auth.Commands.Login;

using MediatR;
using Microsoft.AspNetCore.Identity;
using FlowTask.Application.Features.Auth.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Identity;
using FlowTask.Shared.Results;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResultDto>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(UserManager<AppUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthResultDto>> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result<AuthResultDto>.Unauthorized("InvalidCredentials");

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
            return Result<AuthResultDto>.Unauthorized("InvalidCredentials");

        var accessToken = await _jwtService.GenerateAccessTokenAsync(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddMinutes(15);

        return Result<AuthResultDto>.Ok(
            new AuthResultDto(user.Id, user.Email!, user.FullName ?? "", accessToken, refreshToken, expiresAt));
    }
}