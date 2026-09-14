namespace FlowTask.Infrastructure;

using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using FlowTask.Domain.Identity;
using FlowTask.Infrastructure.Authentication;
using FlowTask.Infrastructure.Identity;
using FlowTask.Infrastructure.Persistence;
using FlowTask.Infrastructure.Services;
using FlowTask.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Diagnostics;
using FlowTask.Application.Features.Auth.OAuth;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── Database ───────────────────────────────────────────
   services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure(3))
    .ConfigureWarnings(w => 
        w.Ignore(RelationalEventId.PendingModelChangesWarning)));

        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<AppDbContext>());

        // ── Identity ───────────────────────────────────────────
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        // ── JWT Auth ───────────────────────────────────────────
        var jwtSettings = configuration.GetSection("Jwt");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = jwtSettings["Issuer"],
                ValidAudience            = jwtSettings["Audience"],
                IssuerSigningKey         = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
                ClockSkew = TimeSpan.FromMinutes(2)
            };
        });

        // ── Services ───────────────────────────────────────────
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IRoleSeeder, RoleSeeder>();
        services.AddScoped<IOAuthUserProvisioningService, UserOAuthProvisioningService>();

        services.AddHttpContextAccessor();

        return services;
    }
}