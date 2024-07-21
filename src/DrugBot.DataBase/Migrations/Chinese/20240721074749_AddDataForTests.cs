using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DrugBot.DataBase.Migrations.Chinese
{
    /// <inheritdoc />
    public partial class AddDataForTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ChineseSymbols",
                columns: new[] { "Id", "ChineseSymbol" },
                values: new object[,]
                {
                    { 1, '安' },
                    { 2, '吧' },
                    { 3, '八' },
                    { 4, '爸' },
                    { 5, '百' },
                    { 6, '北' },
                    { 7, '不' },
                    { 8, '大' },
                    { 9, '岛' },
                    { 10, '的' }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChineseSymbols",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ChineseSymbols",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ChineseSymbols",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ChineseSymbols",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ChineseSymbols",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ChineseSymbols",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ChineseSymbols",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ChineseSymbols",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ChineseSymbols",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ChineseSymbols",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
