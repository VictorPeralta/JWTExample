using Microsoft.EntityFrameworkCore.Migrations;

namespace JWTExample.Migrations
{
    public partial class Activity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ActivityLog",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLog_UserId",
                table: "ActivityLog",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityLog_AspNetUsers_UserId",
                table: "ActivityLog",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityLog_AspNetUsers_UserId",
                table: "ActivityLog");

            migrationBuilder.DropIndex(
                name: "IX_ActivityLog_UserId",
                table: "ActivityLog");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ActivityLog");
        }
    }
}
