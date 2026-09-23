using JobManagement.Domain.Enums;

namespace JobManagement.Application.DTOs.Job;

public class CreateJobDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public JobType JobType { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public string Location { get; set; }
    public int NumberOfVacancies { get; set; }
    public string Salary { get; set; }
    public string Responsibilities { get; set; }
    public string Requirements { get; set; }
    public DateTime ApplicationDeadline { get; set; }
}
