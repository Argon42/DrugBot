using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DrugBot.DataBase.Migrations.MagicBall
{
    /// <inheritdoc />
    public partial class AddDataForTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Prediction",
                table: "Answers",
                newName: "Answer");

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "Answer" },
                values: new object[,]
                {
                    { 1, "ДА" },
                    { 2, "очень вероятно" },
                    { 3, "безусловно" },
                    { 4, "без сомнений" },
                    { 5, "должно быть так" },
                    { 6, "абсолютно точно" },
                    { 7, "мне кажется да" },
                    { 8, "духи говорят да" },
                    { 9, "похоже, что да" },
                    { 10, "НЕТ" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Answers",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.RenameColumn(
                name: "Answer",
                table: "Answers",
                newName: "Prediction");
        }
    }
}
