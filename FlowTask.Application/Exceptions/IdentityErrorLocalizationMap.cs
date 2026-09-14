// FlowTask.Application/Exceptions/IdentityErrorLocalizationMap.cs
namespace FlowTask.Application.Exceptions;

/// <summary>Maps ASP.NET Identity's stable IdentityError.Code values to keys
/// in the "Validation" resource file, so results.Errors never leak raw English.</summary>
public static class IdentityErrorLocalizationMap
{
    public static string KeyFor(string identityCode) => identityCode switch
    {
        "PasswordTooShort"           => "PasswordTooShort",
        "PasswordRequiresDigit"      => "PasswordRequiresDigit",
        "PasswordRequiresUpper"      => "PasswordRequiresUpper",
        "PasswordRequiresLower"      => "PasswordRequiresLower",
        "PasswordRequiresNonAlphanumeric" => "PasswordRequiresSpecialChar",
        "DuplicateUserName"          => "EmailAlreadyExists",
        "InvalidEmail"               => "EmailFormat",
        _                            => "ValidationError",
    };
}