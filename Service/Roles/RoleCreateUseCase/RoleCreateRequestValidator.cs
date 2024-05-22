using FluentValidation;

namespace Service.Roles.RoleCreateUseCase;

public class RoleCreateRequestValidator : AbstractValidator<RoleCreateRequestDto>
{
    public RoleCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .NotNull().WithMessage("{PropertyName} is required")
            .Length(2, 25).WithMessage("{PropertyName} must be between 5 and 25 characters");
    }
}
