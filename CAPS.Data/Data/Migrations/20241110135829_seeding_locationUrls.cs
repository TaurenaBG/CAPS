using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAPS.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class seeding_locationUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PawnShops",
                keyColumn: "Id",
                keyValue: 1,
                column: "LocationUrl",
                value: "https://www.google.com/maps?q=43.2141,27.9147");

            migrationBuilder.UpdateData(
                table: "PawnShops",
                keyColumn: "Id",
                keyValue: 2,
                column: "LocationUrl",
                value: "https://www.google.com/maps?q=42.1354,24.7455");

            migrationBuilder.UpdateData(
                table: "PawnShops",
                keyColumn: "Id",
                keyValue: 3,
                column: "LocationUrl",
                value: "https://www.google.com/maps?q=42.6977,23.3219");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PawnShops",
                keyColumn: "Id",
                keyValue: 1,
                column: "LocationUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "PawnShops",
                keyColumn: "Id",
                keyValue: 2,
                column: "LocationUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "PawnShops",
                keyColumn: "Id",
                keyValue: 3,
                column: "LocationUrl",
                value: null);
        }
    }
}
