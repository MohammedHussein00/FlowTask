namespace FlowTask.Application.Features.Workspaces.Validators;

using FluentValidation;
using FlowTask.Application.Features.Workspaces.Commands.CreateWorkspace;

public class CreateWorkspaceCommandValidator : AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Workspace name is required.")
            .MaximumLength(100).WithMessage("Workspace name cannot exceed 100 characters.");

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(500)
            .Must(url => url is null || Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Avatar URL must be a valid URL.");
    }
}