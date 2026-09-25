using FluentValidation;
using JobManagement.Application.DTOs.CompanyJoinRequest;

namespace JobManagement.Application.Validations.CompanyJoinRequest;

public class ChangeCompanyJoinRequestStatusValidator : AbstractValidator<ChangeCompanyJoinRequestStatusDto>
{
    public ChangeCompanyJoinRequestStatusValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");


        RuleFor(x => x.Status)
           .IsInEnum().WithMessage("Allowed values: Pending, Accepted, Rejected.");
    }
}
