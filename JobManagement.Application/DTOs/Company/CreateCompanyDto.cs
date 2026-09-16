

namespace JobManagement.Application.DTOs.Company
{
    public class CreateCompanyDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Website { get; set; }
    }
}
