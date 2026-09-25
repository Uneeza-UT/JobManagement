using FluentValidation;
using JobManagement.Application.DTOs.CompanyJoinRequest;

namespace JobManagement.Application.Validations.CompanyJoinRequest;

public class CreateCompanyJoinRequestValidator : AbstractValidator<CreateCompanyJoinRequestDto>
{
    public CreateCompanyJoinRequestValidator()
    {

        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
    }
}
