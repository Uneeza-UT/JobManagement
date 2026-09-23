using JobManagement.Domain.Enums;

namespace JobManagement.Application.DTOs.JobApplication;

public class ChangeJobApplicationStatusDto
{
    public int Id { get; set; }
    public JobApplicationStatus Status { get; set; }
}
