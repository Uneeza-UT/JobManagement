using FluentValidation.Results;

namespace JobManagement.Application.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
        
    }

    public BadRequestException(string message, ValidationResult validationResult) : base(message)
    {
        ValidationsErrors = validationResult.ToDictionary();    
    }

    public IDictionary<string, string[]> ValidationsErrors { get; set; }
}
