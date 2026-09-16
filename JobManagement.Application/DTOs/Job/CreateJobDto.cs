using JobManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobManagement.Application.DTOs.Job
{
    public class CreateJobDto
    {
        public string Type { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NumberOfVacancies { get; set; }
        public string Salary { get; set; }
        public string Responsibilities { get; set; }
        public string Requirements { get; set; }
        public DateTime ApplicationDeadline { get; set; }
    }
}
