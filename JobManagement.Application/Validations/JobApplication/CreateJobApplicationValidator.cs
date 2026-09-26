using FluentValidation;
using JobManagement.Application.DTOs.JobApplication;

namespace JobManagement.Application.Validations.JobApplication;

public class CreateJobApplicationValidator : AbstractValidator<CreateJobApplicationDto>
{
    public CreateJobApplicationValidator()
    {
        RuleFor(x => x.FirstName)
           .NotEmpty().WithMessage("{PropertyName} is required.")
           .MaximumLength(200).WithMessage("{PropertyName} cannot exceed 200 characters.");


        RuleFor(x => x.LastName)
           .NotEmpty().WithMessage("{PropertyName} is required.")
           .MaximumLength(200).WithMessage("{PropertyName} cannot exceed 200 characters.");


        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Matches(@"^[^@\s]+@[^@\s.]+(\.[^@\s.]+)+$").WithMessage("{PropertyName} must be a valid email address.")
            .MaximumLength(500).WithMessage("{PropertyName} cannot exceed 500 characters.");


        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Matches(@"^\+[1-9]\d{7,14}$")
            .WithMessage("{PropertyName} must be a valid international phone number in E.164 format. Example, +92XXXXXXXXXX")
            .MaximumLength(25).WithMessage("{PropertyName} cannot exceed 25 characters.");


        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(200).WithMessage("{PropertyName} cannot exceed 200 characters.");



        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Allowed values: Male, Female, Other.");


        RuleFor(x => x.BirthDate)
            .LessThanOrEqualTo(DateTime.UtcNow.Date)
            .When(x => x.BirthDate.HasValue)
            .WithMessage("{PropertyName} cannot be in the future.");

 
        RuleFor(x => x.ApplicationDocument)
            .Must(file => file == null || file.ContentType == "application/pdf")
            .WithMessage("Application Document must be a PDF.");



        RuleFor(x => x.CNIC)
            .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");


        RuleFor(x => x.JobId)
            .NotEmpty(). WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

    }
}
