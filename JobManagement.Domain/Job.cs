using JobManagement.Domain.Enums;
using JobManagement.Domain.Shared;


namespace JobManagement.Domain;

public class Job : BaseEntity
{
    public string Type { get; set; }
    public string Location { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int NumberOfVacancies { get; set; }
    public string Salary { get; set; }
    public string Responsibilities { get; set; }
    public string Requirements { get; set; }
    public JobApprovalStatus ApprovalStatus { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

}
