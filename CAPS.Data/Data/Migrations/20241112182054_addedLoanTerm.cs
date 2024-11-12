using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAPS.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedLoanTerm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanTerm",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanTerm",
                table: "Loans");
        }
    }
}
