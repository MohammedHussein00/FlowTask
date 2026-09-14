namespace FlowTask.Api.Localization.Services;

using Microsoft.Extensions.Localization;
using FlowTask.Shared.Localization;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizerFactory _factory;

    public LocalizationService(IStringLocalizerFactory factory)
        => _factory = factory;

    public string Get(string resourceKey, string? resourceFile = null)
    {
        var baseName = ResolveBaseName(resourceFile ?? "Common");
        var localizer = _factory.Create(baseName, typeof(LocalizationService).Assembly.GetName().Name!);
        return localizer[resourceKey].Value;
    }

    public string Get(string resourceKey, string resourceFile, params object[] args)
    {
        var baseName = ResolveBaseName(resourceFile);
        var localizer = _factory.Create(baseName, typeof(LocalizationService).Assembly.GetName().Name!);
        return string.Format(localizer[resourceKey].Value, args);
    }

    // "Auth" -> "FlowTask.Api.Auth.Auth"
    // AddLocalization(ResourcesPath = "Localization") inserts "Localization." for us,
    // producing the real resource name: FlowTask.Api.Localization.Auth.Auth
    private static string ResolveBaseName(string resourceFile) =>
        $"FlowTask.Api.{resourceFile}.{resourceFile}";
}