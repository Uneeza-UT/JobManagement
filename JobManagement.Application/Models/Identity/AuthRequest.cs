using System.ComponentModel.DataAnnotations;

namespace JobManagement.Application.Models.Identity;

public class AuthRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}
