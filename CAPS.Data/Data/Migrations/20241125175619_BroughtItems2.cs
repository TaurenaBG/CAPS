using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAPS.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class BroughtItems2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "PawnItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PawnItems_AppUserId1",
                table: "PawnItems",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PawnItems_AspNetUsers_AppUserId1",
                table: "PawnItems",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PawnItems_AspNetUsers_AppUserId1",
                table: "PawnItems");

            migrationBuilder.DropIndex(
                name: "IX_PawnItems_AppUserId1",
                table: "PawnItems");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "PawnItems");
        }
    }
}
