using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CAPS.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class clearStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PawnShops",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PawnShops",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PawnShops",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PawnShops",
                columns: new[] { "Id", "City", "IsDeleted", "LocationUrl", "Name" },
                values: new object[,]
                {
                    { 1, "Varna", false, "https://www.google.com/maps?q=43.2141,27.9147", "GoldenVarna" },
                    { 2, "Plovdiv", false, "https://www.google.com/maps?q=42.1354,24.7455", "MainaShop" },
                    { 3, "Sofia", false, "https://www.google.com/maps?q=42.6977,23.3219", "Viliger" }
                });
        }
    }
}
