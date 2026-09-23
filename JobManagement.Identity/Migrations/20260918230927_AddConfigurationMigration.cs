using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobManagement.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "j6vw1qx3-5f2k-7b0p-9wn2-5h2w0a8t2ry6", "143eb682-35a8-43fc-a4bf-a3f37857ea4b", "Company", "COMPANY" },
                    { "k8mz4rp2-7x1c-5vn9-3qwf-6t2h8y0p4sa", "eacee57d-39ae-4e70-b8ff-ad4da940d2b8", "Student", "STUDENT" },
                    { "t2ne0ux3-9a53-6d1l-7vm1-4f8w3s7h2gp0", "4f13e0f4-b9aa-42cf-a0a0-43218c4a3e93", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "b5qy7nx1-2vm8-9rk3-4wz6-1h0p5t7c9fd", 0, "cf6115f6-53ab-4002-83a8-d4423c1c8fa2", "admin@example.com", true, "System", "Admin", false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEGmkWaKXkQ59hMc56BfFMArmrOExMatE7OAGqsGN19sPpO0VRBbmFp2E21sgWVyFlw==", null, false, "7d0a7648-8761-4612-b1c5-2bc19089f2f0", false, "admin@example.com" },
                    { "p3ls6wd8-1qv5-7kx2-9mza-4r8n2y6h0tc", 0, "ef65a597-ffdc-47bc-8775-d00ad46a6ac0", "company@example.com", true, "Test", "Company", false, null, "COMPANY@EXAMPLE.COM", "COMPANY@EXAMPLE.COM", "AQAAAAIAAYagAAAAELwOk0RoG5ZhtJUjF6VcZaKk0O2PqPmMIXpzFgi/1DMMqVrRY770bLi/Llzkf5FfOw==", null, false, "5c165a5b-e0cd-48da-91cd-a15f9530c2e2", false, "company@example.com" },
                    { "v7k2m9qx-4r5t-8nw1-6pz3-h0y7c2a5fds", 0, "d3671d2b-9066-42fd-9758-2dd050d340d0", "student@example.com", true, "Test", "Student", false, null, "STUDENT@EXAMPLE.COM", "STUDENT@EXAMPLE.COM", "AQAAAAIAAYagAAAAEN0v5ZSXb6sUcEb3JlPo6FHS2gfP69WWXMLUlJC958Cxig2L31oArDhQdQ4KhYXMkg==", null, false, "dae0c34a-d6e4-49cc-8d5e-c372af33b2d0", false, "student@example.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "t2ne0ux3-9a53-6d1l-7vm1-4f8w3s7h2gp0", "b5qy7nx1-2vm8-9rk3-4wz6-1h0p5t7c9fd" },
                    { "j6vw1qx3-5f2k-7b0p-9wn2-5h2w0a8t2ry6", "p3ls6wd8-1qv5-7kx2-9mza-4r8n2y6h0tc" },
                    { "k8mz4rp2-7x1c-5vn9-3qwf-6t2h8y0p4sa", "v7k2m9qx-4r5t-8nw1-6pz3-h0y7c2a5fds" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "t2ne0ux3-9a53-6d1l-7vm1-4f8w3s7h2gp0", "b5qy7nx1-2vm8-9rk3-4wz6-1h0p5t7c9fd" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "j6vw1qx3-5f2k-7b0p-9wn2-5h2w0a8t2ry6", "p3ls6wd8-1qv5-7kx2-9mza-4r8n2y6h0tc" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "k8mz4rp2-7x1c-5vn9-3qwf-6t2h8y0p4sa", "v7k2m9qx-4r5t-8nw1-6pz3-h0y7c2a5fds" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "j6vw1qx3-5f2k-7b0p-9wn2-5h2w0a8t2ry6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "k8mz4rp2-7x1c-5vn9-3qwf-6t2h8y0p4sa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "t2ne0ux3-9a53-6d1l-7vm1-4f8w3s7h2gp0");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b5qy7nx1-2vm8-9rk3-4wz6-1h0p5t7c9fd");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "p3ls6wd8-1qv5-7kx2-9mza-4r8n2y6h0tc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "v7k2m9qx-4r5t-8nw1-6pz3-h0y7c2a5fds");
        }
    }
}
