using FluentValidation;
using JobManagement.Application.DTOs.JobApplication;

namespace JobManagement.Application.Validations.JobApplication;

public class ChangeJobApplicationStatusValidator : AbstractValidator<ChangeJobApplicationStatusDto>
{
    public ChangeJobApplicationStatusValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Allowed values: Applied, Shortlisted, InterviewScheduled, Hired, Rejected.");
    }
}
