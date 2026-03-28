using FluentValidation;
using FlowDesk.Services.DTOs;

namespace FlowDesk.Services.Validators;

public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .NotNull().WithMessage("Title cannot be null")
            .Length(1, 255).WithMessage("Title must be between 1 and 255 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(1).WithMessage("Priority must be at least 1")
            .LessThanOrEqualTo(5).WithMessage("Priority cannot exceed 5");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date cannot be in the past")
            .When(x => x.DueDate.HasValue);

        RuleFor(x => x.ProjectId)
            .GreaterThan(0).WithMessage("Valid ProjectId is required");

        RuleFor(x => x.AssignedToUserId)
            .GreaterThan(0).WithMessage("AssignedToUserId must be greater than 0")
            .When(x => x.AssignedToUserId.HasValue);
    }
}

public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be empty")
            .Length(1, 255).WithMessage("Title must be between 1 and 255 characters")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(1).WithMessage("Priority must be at least 1")
            .LessThanOrEqualTo(5).WithMessage("Priority cannot exceed 5")
            .When(x => x.Priority.HasValue);

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date cannot be in the past")
            .When(x => x.DueDate.HasValue);

        RuleFor(x => x.AssignedToUserId)
            .GreaterThan(0).WithMessage("AssignedToUserId must be greater than 0")
            .When(x => x.AssignedToUserId.HasValue);
    }
}
