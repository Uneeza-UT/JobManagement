using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobManagement.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompanyIdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "j6vw1qx3-5f2k-7b0p-9wn2-5h2w0a8t2ry6",
                column: "ConcurrencyStamp",
                value: "2ff6564c-30dd-40e9-9dcd-06a167333b37");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "k8mz4rp2-7x1c-5vn9-3qwf-6t2h8y0p4sa",
                column: "ConcurrencyStamp",
                value: "a3e86bbf-cd0b-46e2-9b03-4adc53117aae");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "t2ne0ux3-9a53-6d1l-7vm1-4f8w3s7h2gp0",
                column: "ConcurrencyStamp",
                value: "f643be60-e1ef-4701-abd8-d2e4a171809f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b5qy7nx1-2vm8-9rk3-4wz6-1h0p5t7c9fd",
                columns: new[] { "CompanyId", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { null, "ae7c687b-818d-45a0-98fa-a0a517731390", "AQAAAAIAAYagAAAAEElGeKAcUfTuPoaVAZa3eiUksUrzqw+YjGErlyYS6BRVg1Ddsp8cZ6isR5aRG0XjBA==", "0c05e687-81d7-4a4f-9fb2-cc6204145c4f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "p3ls6wd8-1qv5-7kx2-9mza-4r8n2y6h0tc",
                columns: new[] { "CompanyId", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { null, "24c2060d-df17-4792-be29-ac708c3a02e3", "AQAAAAIAAYagAAAAEIRRXRNhe3vaL7iwNZC179V/GfL2q9NLPXTFiRWJqTuuaAEcvNCK3Ro7pJ2H5BWjJw==", "ab4ae4d9-3577-4e70-89af-084b1ee379cd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "v7k2m9qx-4r5t-8nw1-6pz3-h0y7c2a5fds",
                columns: new[] { "CompanyId", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { null, "77c0b0ea-dfa8-4c3e-a380-de72c66d0f12", "AQAAAAIAAYagAAAAEHV3wuPjPIkTI/wAUDMNq9XIkuHIKtJUxvXlBrhoGp8BP+cHcIsiuJDz4U5Mvjc2rw==", "4d85e02b-3cdf-4544-b895-eb4ee8d3f837" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "j6vw1qx3-5f2k-7b0p-9wn2-5h2w0a8t2ry6",
                column: "ConcurrencyStamp",
                value: "143eb682-35a8-43fc-a4bf-a3f37857ea4b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "k8mz4rp2-7x1c-5vn9-3qwf-6t2h8y0p4sa",
                column: "ConcurrencyStamp",
                value: "eacee57d-39ae-4e70-b8ff-ad4da940d2b8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "t2ne0ux3-9a53-6d1l-7vm1-4f8w3s7h2gp0",
                column: "ConcurrencyStamp",
                value: "4f13e0f4-b9aa-42cf-a0a0-43218c4a3e93");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b5qy7nx1-2vm8-9rk3-4wz6-1h0p5t7c9fd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cf6115f6-53ab-4002-83a8-d4423c1c8fa2", "AQAAAAIAAYagAAAAEGmkWaKXkQ59hMc56BfFMArmrOExMatE7OAGqsGN19sPpO0VRBbmFp2E21sgWVyFlw==", "7d0a7648-8761-4612-b1c5-2bc19089f2f0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "p3ls6wd8-1qv5-7kx2-9mza-4r8n2y6h0tc",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ef65a597-ffdc-47bc-8775-d00ad46a6ac0", "AQAAAAIAAYagAAAAELwOk0RoG5ZhtJUjF6VcZaKk0O2PqPmMIXpzFgi/1DMMqVrRY770bLi/Llzkf5FfOw==", "5c165a5b-e0cd-48da-91cd-a15f9530c2e2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "v7k2m9qx-4r5t-8nw1-6pz3-h0y7c2a5fds",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d3671d2b-9066-42fd-9758-2dd050d340d0", "AQAAAAIAAYagAAAAEN0v5ZSXb6sUcEb3JlPo6FHS2gfP69WWXMLUlJC958Cxig2L31oArDhQdQ4KhYXMkg==", "dae0c34a-d6e4-49cc-8d5e-c372af33b2d0" });
        }
    }
}
