using FlowTask.Application;
using FlowTask.Infrastructure;
using FlowTask.Infrastructure.Identity;
using FlowTask.Infrastructure.Persistence;
using FlowTask.Infrastructure.ExternalAuth;
using FlowTask.Application.Features.Auth.OAuth;
using FlowTask.Api.GraphQL;
using FlowTask.Api.GraphQL.Filters;
using FlowTask.Api.Localization.Services;
using FlowTask.Api.Middlewares;
using FlowTask.Shared.Localization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Layers ──────────────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ── Localization ─────────────────────────────────────────────
builder.Services.AddLocalization(opts => opts.ResourcesPath = "Localization");
builder.Services.AddScoped<ILocalizationService, LocalizationService>();


builder.Services.Configure<GoogleOAuthOptions>(
    builder.Configuration.GetSection("OAuth:Google"));
builder.Services.Configure<GitHubOAuthOptions>(
    builder.Configuration.GetSection("OAuth:GitHub"));

builder.Services.AddHttpClient<IOAuthProviderService, GoogleOAuthService>();
builder.Services.AddHttpClient<IOAuthProviderService, GitHubOAuthService>();

builder.Services
    .AddGraphQLServer()
    .ModifyRequestOptions(o => o.IncludeExceptionDetails = builder.Environment.IsDevelopment())
    .AddAuthorization()
    .AddErrorFilter<ErrorFilter>()
    .AddQueryType(d => d.Name("Query"))
    .AddMutationType(d => d.Name("Mutation"))
    .AddTypeExtensionsFromAssembly(typeof(Program).Assembly)
    .AddProjections()
    .AddFiltering()
    .AddSorting();

// ── CORS ─────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddPolicy("FlowTaskClient", policy =>
        policy.WithOrigins(
                builder.Configuration
                    .GetSection("Cors:Origins")
                    .Get<string[]>() ?? ["http://localhost:4200"])
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── Seed ─────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    var seeder = scope.ServiceProvider.GetRequiredService<IRoleSeeder>();
    await seeder.SeedAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ── Middleware ────────────────────────────────────────────────
app.UseMiddleware<CultureMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("FlowTaskClient");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGraphQL("/graphql");

app.Run();

public partial class Program { }
