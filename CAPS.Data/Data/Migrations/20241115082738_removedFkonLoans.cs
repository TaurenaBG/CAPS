using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAPS.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class removedFkonLoans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_PawnShops_PawnShopId",
                table: "Loans");

            migrationBuilder.AlterColumn<int>(
                name: "PawnShopId",
                table: "Loans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_PawnShops_PawnShopId",
                table: "Loans",
                column: "PawnShopId",
                principalTable: "PawnShops",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_PawnShops_PawnShopId",
                table: "Loans");

            migrationBuilder.AlterColumn<int>(
                name: "PawnShopId",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_PawnShops_PawnShopId",
                table: "Loans",
                column: "PawnShopId",
                principalTable: "PawnShops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
