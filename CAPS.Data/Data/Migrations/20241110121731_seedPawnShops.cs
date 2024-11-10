using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CAPS.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class seedPawnShops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PawnShops",
                columns: new[] { "Id", "City", "LocationUrl", "Name" },
                values: new object[,]
                {
                    { 1, "Varna", null, "GoldenVarna" },
                    { 2, "Plovdiv", null, "MainaShop" },
                    { 3, "Sofia", null, "Viliger" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
