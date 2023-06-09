using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "PermissionRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRequests_RoleId",
                table: "PermissionRequests",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRequests_AspNetRoles_RoleId",
                table: "PermissionRequests",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRequests_AspNetRoles_RoleId",
                table: "PermissionRequests");

            migrationBuilder.DropIndex(
                name: "IX_PermissionRequests_RoleId",
                table: "PermissionRequests");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "PermissionRequests");
        }
    }
}
