using FluentValidation;
using JobManagement.Application.Models.Identity;

namespace JobManagement.Application.Validations.Identity;

public class AuthRequestValidator : AbstractValidator<AuthRequest>
{
    public AuthRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Matches(@"^[^@\s]+@[^@\s.]+(\.[^@\s.]+)+$").WithMessage("{PropertyName} must be a valid email address.");


        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(8).WithMessage("{PropertyName} must be atleast 8 characters.");

    }
}
