using JobManagement.Domain;
using JobManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobManagement.Application.DTOs.JobApplication
{
    public class ChangeJobApplicationStatus
    {
        public int Id { get; set; }
        public JobApplicationStatus Status { get; set; }
    }
}
