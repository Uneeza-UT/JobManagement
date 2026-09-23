using JobManagement.Domain.Enums;

namespace JobManagement.Application.DTOs.Job;

public class ChangeJobApprovalStatusDto
{
    public int Id { get; set; }
    public JobApprovalStatus ApprovalStatus { get; set; }
}
