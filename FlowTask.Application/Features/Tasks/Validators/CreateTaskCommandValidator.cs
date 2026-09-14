namespace FlowTask.Application.Features.Tasks.Validators;

using FluentValidation;
using FlowTask.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    private static readonly string[] ValidPriorities = ["urgent", "high", "normal", "low"];

    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0).WithMessage("List ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Task name is required.")
            .MaximumLength(255).WithMessage("Task name cannot exceed 255 characters.");

        RuleFor(x => x.Priority)
            .Must(p => ValidPriorities.Contains(p.ToLower()))
            .WithMessage($"Priority must be one of: {string.Join(", ", ValidPriorities)}");

        RuleFor(x => x.DueDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.DueDate.HasValue)
            .WithMessage("Due date must be after start date.");
    }
}