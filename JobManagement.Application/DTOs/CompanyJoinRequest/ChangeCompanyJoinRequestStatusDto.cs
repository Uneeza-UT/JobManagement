using JobManagement.Domain.Enums;

namespace JobManagement.Application.DTOs.CompanyJoinRequest;

public class ChangeCompanyJoinRequestStatusDto
{
    public int Id { get; set; }
    public JoinRequestStatus Status { get; set; }
}
