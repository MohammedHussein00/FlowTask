namespace FlowTask.Application.Interfaces;

using FlowTask.Domain.Identity;

public interface IJwtService
{
    Task<string> GenerateAccessTokenAsync(AppUser user);
    string GenerateRefreshToken();
}