using FluentValidation;
using JobManagement.Application.DTOs.Job;

namespace JobManagement.Application.Validations.Job;

public class ChangeJobApprovalStatusValidator : AbstractValidator<ChangeJobApprovalStatusDto>
{
    public ChangeJobApprovalStatusValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Job Id must be greater than zero.");


        RuleFor(x => x.ApprovalStatus)
            .IsInEnum().WithMessage("Invalid job approval status.");
    }
    
}
