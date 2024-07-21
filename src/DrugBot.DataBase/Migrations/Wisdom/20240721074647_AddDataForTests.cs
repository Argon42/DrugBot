using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DrugBot.DataBase.Migrations.Wisdom
{
    /// <inheritdoc />
    public partial class AddDataForTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Wisdoms",
                columns: new[] { "Id", "Wisdom" },
                values: new object[,]
                {
                    { 1, "Что разум человека может постигнуть и во что он может поверить, того он способен достичь" },
                    { 2, "Стремитесь не к успеху, а к ценностям, которые он дает​" },
                    { 3, "Своим успехом я обязана тому, что никогда не оправдывалась и не принимала оправданий от других." },
                    { 4, "За свою карьеру я пропустил более 9000 бросков, проиграл почти 300 игр. 26 раз мне доверяли сделать финальный победный бросок, и я промахивался. Я терпел поражения снова, и снова, и снова. И именно поэтому я добился успеха." },
                    { 5, "Сложнее всего начать действовать, все остальное зависит только от упорства." },
                    { 6, "Надо любить жизнь больше, чем смысл жизни." },
                    { 7, "Жизнь - это то, что с тобой происходит, пока ты строишь планы." },
                    { 8, "Логика может привести Вас от пункта А к пункту Б, а воображение — куда угодно." },
                    { 9, "Через 20 лет вы будете больше разочарованы теми вещами, которые вы не делали, чем теми, которые вы сделали. Так отчальте от тихой пристани. Почувствуйте попутный ветер в вашем парусе. Двигайтесь вперед, действуйте, открывайте!" },
                    { 10, "Начинать всегда стоит с того, что сеет сомнения." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Wisdoms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Wisdoms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Wisdoms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Wisdoms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Wisdoms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Wisdoms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Wisdoms",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Wisdoms",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Wisdoms",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Wisdoms",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
