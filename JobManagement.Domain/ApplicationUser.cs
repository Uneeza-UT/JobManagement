using System;
using System.Collections.Generic;
using System.Text;

namespace JobManagement.Domain;

public class ApplicationUser
{
    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}

