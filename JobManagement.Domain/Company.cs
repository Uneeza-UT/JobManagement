using JobManagement.Domain.Shared;

namespace JobManagement.Domain;

public class Company : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? Website { get; set; }
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}
