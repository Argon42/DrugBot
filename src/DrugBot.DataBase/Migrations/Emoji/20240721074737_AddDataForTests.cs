using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DrugBot.DataBase.Migrations.Emoji
{
    /// <inheritdoc />
    public partial class AddDataForTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Emojis",
                columns: new[] { "Id", "Emoji" },
                values: new object[,]
                {
                    { 1, "&#128512;" },
                    { 2, "&#128515;" },
                    { 3, "&#128516;" },
                    { 4, "&#128513;" },
                    { 5, "&#128518;" },
                    { 6, "&#128517;" },
                    { 7, "&#129315;" },
                    { 8, "&#128514;" },
                    { 9, "&#128578;" },
                    { 10, "&#128579;" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Emojis",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Emojis",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Emojis",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Emojis",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Emojis",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Emojis",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Emojis",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Emojis",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Emojis",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Emojis",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
