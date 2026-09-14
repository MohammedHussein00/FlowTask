namespace FlowTask.Application.Features.Auth.Validators;

using FluentValidation;
using FlowTask.Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithErrorCode("FullNameRequired")
            .MaximumLength(100).WithErrorCode("FullNameMaxLength");

        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("EmailRequired")
            .EmailAddress().WithErrorCode("EmailFormat");

        RuleFor(x => x.Password)
            .NotEmpty().WithErrorCode("PasswordRequired")
            .MinimumLength(8).WithErrorCode("PasswordTooShort")
            .Matches("[A-Z]").WithErrorCode("PasswordRequiresUpper")
            .Matches("[0-9]").WithErrorCode("PasswordRequiresDigit")
            .Matches("[^a-zA-Z0-9]").WithErrorCode("PasswordRequiresSpecialChar");
    }
}