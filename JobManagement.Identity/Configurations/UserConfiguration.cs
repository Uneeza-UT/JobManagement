using JobManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManagement.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            builder.HasData(
                new ApplicationUser
                {
                    Id = "b5qy7nx1-2vm8-9rk3-4wz6-1h0p5t7c9fd",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    FirstName = "System",
                    LastName = "Admin",
                    UserName = "admin@example.com",
                    NormalizedUserName = "ADMIN@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Admin1234!"),
                    EmailConfirmed = true
                },
                new ApplicationUser
                {
                    Id = "p3ls6wd8-1qv5-7kx2-9mza-4r8n2y6h0tc",
                    Email = "company@example.com",
                    NormalizedEmail = "COMPANY@EXAMPLE.COM",
                    FirstName = "Test",
                    LastName = "Company",
                    UserName = "company@example.com",
                    NormalizedUserName = "COMPANY@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Company1234!"),
                    EmailConfirmed = true
                },
                new ApplicationUser
                {
                    Id = "v7k2m9qx-4r5t-8nw1-6pz3-h0y7c2a5fds",
                    Email = "student@example.com",
                    NormalizedEmail = "STUDENT@EXAMPLE.COM",
                    FirstName = "Test",
                    LastName = "Student",
                    UserName = "student@example.com",
                    NormalizedUserName = "STUDENT@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Student1234!"),
                    EmailConfirmed = true
                }
            );
        }

    }
}
