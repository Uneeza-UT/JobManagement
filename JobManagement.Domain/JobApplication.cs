using JobManagement.Domain.Enums;
using JobManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobManagement.Domain;

public class JobApplication
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public DateTime BirthDate { get; set; }
    public string? ApplicationDocumentKey { get; set; }
    public JobApplicationStatus Status { get; set; }
    public int JobId { get; set; }
    public Job? Job { get; set; } 
    public int ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; } 
    public DateTime DateApplied { get; set; }
}
