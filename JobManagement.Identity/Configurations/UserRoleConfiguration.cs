using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobManagement.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "t2ne0ux3-9a53-6d1l-7vm1-4f8w3s7h2gp0",
                    UserId = "b5qy7nx1-2vm8-9rk3-4wz6-1h0p5t7c9fd"
                },

                new IdentityUserRole<string>
                {
                    RoleId = "j6vw1qx3-5f2k-7b0p-9wn2-5h2w0a8t2ry6",
                    UserId = "p3ls6wd8-1qv5-7kx2-9mza-4r8n2y6h0tc"
                },

                new IdentityUserRole<string>
                {
                    RoleId = "k8mz4rp2-7x1c-5vn9-3qwf-6t2h8y0p4sa",
                    UserId = "v7k2m9qx-4r5t-8nw1-6pz3-h0y7c2a5fds"
                }
            );
        }
    }
}
