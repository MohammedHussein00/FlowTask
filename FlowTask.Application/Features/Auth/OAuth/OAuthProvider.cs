namespace FlowTask.Application.Features.Auth.OAuth;

/// <summary>Must match the string values the frontend sends in OAuthLoginInput.Provider.</summary>
public enum OAuthProvider
{
    Google,
    GitHub,
}