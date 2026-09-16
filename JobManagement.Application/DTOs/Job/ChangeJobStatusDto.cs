using JobManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobManagement.Application.DTOs.Job
{
    public class ChangeJobStatusDto
    {
        public int Id { get; set; }
        public JobApprovalStatus ApprovalStatus { get; set; }
    }
}
