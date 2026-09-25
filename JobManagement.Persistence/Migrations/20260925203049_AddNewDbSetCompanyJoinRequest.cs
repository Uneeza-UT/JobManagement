using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewDbSetCompanyJoinRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJoinRequest_Companies_CompanyId",
                table: "CompanyJoinRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyJoinRequest",
                table: "CompanyJoinRequest");

            migrationBuilder.RenameTable(
                name: "CompanyJoinRequest",
                newName: "CompanyJoinRequests");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyJoinRequest_CompanyId",
                table: "CompanyJoinRequests",
                newName: "IX_CompanyJoinRequests_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyJoinRequests",
                table: "CompanyJoinRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyJoinRequests_Companies_CompanyId",
                table: "CompanyJoinRequests",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJoinRequests_Companies_CompanyId",
                table: "CompanyJoinRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyJoinRequests",
                table: "CompanyJoinRequests");

            migrationBuilder.RenameTable(
                name: "CompanyJoinRequests",
                newName: "CompanyJoinRequest");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyJoinRequests_CompanyId",
                table: "CompanyJoinRequest",
                newName: "IX_CompanyJoinRequest_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyJoinRequest",
                table: "CompanyJoinRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyJoinRequest_Companies_CompanyId",
                table: "CompanyJoinRequest",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
