namespace FlowTask.Shared.Localization;

public interface ILocalizationService
{
    string Get(string resourceKey, string? resourceFile = null);
    string Get(string resourceKey, string resourceFile, params object[] args);
}