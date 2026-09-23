using FluentValidation;
using JobManagement.Application.DTOs.Company;

namespace JobManagement.Application.Validations.Company;

public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyDto>
{
    public UpdateCompanyValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty()
           .GreaterThan(0).WithMessage("Company {PropertyName} must be greater than zero.");


        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Company {PropertyName} is required.")
           .MaximumLength(500).WithMessage("Company {PropertyName} cannot exceed 500 characters.");


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
