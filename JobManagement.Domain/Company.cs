using JobManagement.Domain.Shared;

namespace JobManagement.Domain;

public class Company : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string? Website { get; set; }
    public string OwnerUserId { get; set; }
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
    public ICollection<CompanyJoinRequest> CompanyJoinRequests { get; set; } = new List<CompanyJoinRequest>();
}
