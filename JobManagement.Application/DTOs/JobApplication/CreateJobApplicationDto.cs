using JobManagement.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace JobManagement.Application.DTOs.JobApplication;

public class CreateJobApplicationDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public IFormFile? ApplicationDocument { get; set; }
    public string? CNIC { get; set; }
    public bool? HasDisability { get; set; }
    public int JobId { get; set; }
}
