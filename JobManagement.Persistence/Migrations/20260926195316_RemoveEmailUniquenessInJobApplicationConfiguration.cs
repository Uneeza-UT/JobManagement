using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailUniquenessInJobApplicationConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobApplications_Email",
                table: "JobApplications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_Email",
                table: "JobApplications",
                column: "Email",
                unique: true);
        }
    }
}
