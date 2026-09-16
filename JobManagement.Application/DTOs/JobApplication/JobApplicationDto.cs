using JobManagement.Domain;
using JobManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobManagement.Application.DTOs.JobApplication
{
    public class JobApplicationDto
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
        public int ApplicationUserId { get; set; }
        public DateTime DateApplied { get; set; }
    }
}
