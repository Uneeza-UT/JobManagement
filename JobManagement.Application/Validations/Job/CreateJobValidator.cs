using FluentValidation;
using JobManagement.Application.DTOs.Job;

namespace JobManagement.Application.Validations.Job;

public class CreateJobValidator : AbstractValidator<CreateJobDto>
{
    public CreateJobValidator()
    {
        RuleFor(x => x.Title)
           .NotEmpty().WithMessage("Job title is required.")
           .NotNull()
           .MaximumLength(500).WithMessage("Job title cannot exceed 500 characters.");


        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Job description is required.")
            .NotNull();


        RuleFor(x => x.JobType)
            .IsInEnum().WithMessage("Allowed values: Remote, Hybrid, Onsite.");


        RuleFor(x => x.EmploymentType)
            .IsInEnum().WithMessage("Allowed values: FullTime, PartTime, Contract, Internship, Temporary.");


        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Job location is required.")
            .NotNull()
            .MaximumLength(200).WithMessage("Job location cannot exceed 200 characters.");


        RuleFor(x => x.NumberOfVacancies)
            .NotEmpty().WithMessage("Number of vacancies is required.")
            .NotNull()
            .GreaterThan(0).WithMessage("Number of vacancies must be greater than zero.");


        RuleFor(x => x.Salary)
            .NotEmpty().WithMessage("Job salary is required.")
            .NotNull()
            .MaximumLength(200).WithMessage("Job salary cannot exceed 200 characters.");


        RuleFor(x => x.Responsibilities)
            .NotEmpty().WithMessage("Job responsibilities are required.")
            .NotNull();


        RuleFor(x => x.Requirements)
            .NotEmpty().WithMessage("Job requirements are required.")
            .NotNull();


        RuleFor(x => x.ApplicationDeadline)
            .GreaterThan(DateTime.Now).WithMessage("Application deadline must be a future date.")
            .NotNull();


    }
}
