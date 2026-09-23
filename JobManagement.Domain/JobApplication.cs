using JobManagement.Domain.Enums;

namespace JobManagement.Domain;

public class JobApplication
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? ApplicationDocumentKey { get; set; }
    public string? CNIC { get; set; }
    public bool? HasDisability { get; set; }
    public DateTime DateApplied { get; set; }
    public JobApplicationStatus Status { get; set; }
    public int JobId { get; set; }
    public Job? Job { get; set; } 
    public string UserId { get; set; }
}
