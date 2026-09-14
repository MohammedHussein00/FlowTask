//// FlowTask.Infrastructure/Identity/UserOAuthProvisioningService.cs
//namespace FlowTask.Infrastructure.Identity;

//using FlowTask.Application.Exceptions;
//using FlowTask.Application.Features.Auth.DTOs;
//using FlowTask.Application.Features.Auth.OAuth;
//using FlowTask.Application.Interfaces;
//using FlowTask.Domain.Identity;
//using Microsoft.AspNetCore.Identity;

//public class UserOAuthProvisioningService : IOAuthUserProvisioningService
//{
//    private readonly UserManager<AppUser> _userManager;
//    private readonly IJwtService _jwtService;

//    public UserOAuthProvisioningService(UserManager<AppUser> userManager, IJwtService jwtService)
//    {
//        _userManager = userManager;
//        _jwtService = jwtService;
//    }

//    public async Task<AuthResultDto> GetOrCreateAndSignInAsync(
//        OAuthProvider provider, OAuthUserInfo info, CancellationToken ct)
//    {
//        var user = await _userManager.FindByEmailAsync(info.Email);

//        if (user is not null)
//        {
//            // SECURITY: password signups currently set EmailConfirmed = true
//            // with no real verification step. If we auto-merge into any
//            // account matching this email, an attacker can pre-register a
//            // victim's email and get logged into that account the moment the
//            // victim signs in with Google/GitHub ("account pre-hijacking").
//            // Until real email verification exists for password signups,
//            // block the merge instead of silently signing in.
//            if (await _userManager.HasPasswordAsync(user))
//                throw new ConflictException("OAuthAccountExistsUsePassword", "Auth", new object[] { user.Email! });
//        }
//        else
//        {
//            user = new AppUser
//            {
//                UserName = info.Email,
//                Email = info.Email,
//                FullName = info.FullName ?? info.Email,
//                EmailConfirmed = info.EmailVerified,
//            };

//            var createResult = await _userManager.CreateAsync(user);
//            if (!createResult.Succeeded)
//                throw new BadRequestException("OAuthAccountCreateFailed", "Auth");

//            await _userManager.AddToRoleAsync(user, "Member");
//        }

//        var accessToken = await _jwtService.GenerateAccessTokenAsync(user);
//        var refreshToken = _jwtService.GenerateRefreshToken();
//        var expiresAt = DateTime.UtcNow.AddMinutes(15);

//        return new AuthResultDto(user.Id, user.Email!, user.FullName ?? "", accessToken, refreshToken, expiresAt);
//    }
//}