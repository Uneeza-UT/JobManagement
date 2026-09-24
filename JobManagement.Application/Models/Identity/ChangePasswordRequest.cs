using System;
using System.Collections.Generic;
using System.Text;

namespace JobManagement.Application.Models.Identity;

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
