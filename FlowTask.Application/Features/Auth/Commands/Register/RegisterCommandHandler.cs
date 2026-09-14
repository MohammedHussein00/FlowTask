using FlowTask.Application.Exceptions;
using FlowTask.Application.Features.Auth.Commands.Register;
using FlowTask.Application.Features.Auth.DTOs;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.Identity;
using FlowTask.Shared.Localization;
using FlowTask.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResultDto>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly ILocalizationService _localizationService;  // Add this

    public RegisterCommandHandler(
        UserManager<AppUser> userManager,
        IJwtService jwtService,
        ILocalizationService localizationService)  // Inject it
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _localizationService = localizationService;
    }

    public async Task<Result<AuthResultDto>> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not null)
            return Result<AuthResultDto>.EmailExists(request.Email);

        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            EmailConfirmed = true,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var fieldErrors = result.Errors
                .GroupBy(_ => "password")
                .ToDictionary(
                    g => g.Key,
                    g => (IReadOnlyList<(string, object[])>)g
                        .Select(e => (IdentityErrorLocalizationMap.KeyFor(e.Code), Array.Empty<object>()))
                        .ToList());

            return Result<AuthResultDto>.Validation(fieldErrors);
        }

        await _userManager.AddToRoleAsync(user, "Member");

        var accessToken = await _jwtService.GenerateAccessTokenAsync(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddMinutes(15);

        return Result<AuthResultDto>.Ok(
            new AuthResultDto(user.Id, user.Email!, user.FullName ?? "", accessToken, refreshToken, expiresAt));
    }
}