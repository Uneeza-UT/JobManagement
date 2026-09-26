using JobManagement.Domain.Enums;

namespace JobManagement.Application.DTOs.CompanyJoinRequest;

public class CompanyJoinRequestDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public JoinRequestStatus Status { get; set; }
    public int CompanyId { get; set; }
}
