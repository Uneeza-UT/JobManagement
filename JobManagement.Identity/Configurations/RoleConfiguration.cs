using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManagement.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "t2ne0ux3-9a53-6d1l-7vm1-4f8w3s7h2gp0",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Id = "j6vw1qx3-5f2k-7b0p-9wn2-5h2w0a8t2ry6",
                    Name = "Company",
                    NormalizedName = "COMPANY"
                },
                new IdentityRole
                {
                    Id = "k8mz4rp2-7x1c-5vn9-3qwf-6t2h8y0p4sa",
                    Name = "Student",
                    NormalizedName = "STUDENT"
                }
            );
        }
    }
}
