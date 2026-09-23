using FluentValidation;
using JobManagement.Application.DTOs.Common;

namespace JobManagement.Application.Validations.Sort;

public class SortValidator : AbstractValidator<SortDto>
{
    public SortValidator()
    {
        RuleFor(x => x.SortBy)
            .IsInEnum()
            .WithMessage("Allowed values: Ascending, Descending, Latest, Oldest");
    }
}
