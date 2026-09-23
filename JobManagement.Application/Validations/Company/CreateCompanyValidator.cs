using FluentValidation;
using JobManagement.Application.DTOs.Company;

namespace JobManagement.Application.Validations.Company;

public class CreateCompanyValidator : AbstractValidator<CreateCompanyDto>
{
    public CreateCompanyValidator()
    {

        RuleFor(x => x.Name)
           .NotEmpty()
           .WithMessage("Company {PropertyName} is required.");


        RuleFor(x => x.Description)
           .NotEmpty()
           .WithMessage("Company {PropertyName} is required.");


        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Company {PropertyName} is required.")
            .EmailAddress()
            .MaximumLength(500).WithMessage("{PropertyName} cannot exceed 500 characters.");


        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Company {PropertyName} is required.")
            .Matches(@"^\+[1-9]\d{7,14}$")
            .WithMessage("{PropertyName} must be a valid international phone number in E.164 format. Example, +92XXXXXXXXXX")
            .MaximumLength(25).WithMessage("{PropertyName} cannot exceed 25 characters.");


        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Company {PropertyName} is required.")
            .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");


        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Company {PropertyName} is required.")
            .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");


        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Company {PropertyName} is required.")
            .MaximumLength(200).WithMessage("{PropertyName} cannot exceed 200 characters.");


        RuleFor(x => x.Website)
            .MaximumLength(2500).WithMessage("{PropertyName} cannot exceed 2500 characters.");
    }
}
