using JobManagement.Domain.Enums;

namespace JobManagement.Domain;

public class CompanyJoinRequest
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }
    public JoinRequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
