using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DrugBot.DataBase.Migrations.Prediction
{
    /// <inheritdoc />
    public partial class AddDataForTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Predictions",
                columns: new[] { "Id", "Prediction" },
                values: new object[,]
                {
                    { 1, "Перед вами прямая дорога к заветной цели. Получится все, что вы задумали." },
                    { 2, "Нужные люди или счастливое и удачное стечение обстоятельств помогут вам добиться желаемого." },
                    { 3, "Препятствия, возникающие одно за другим, могут помешать выполнению ваших планов." },
                    { 4, "Реализация целей зависит от ваших усилий. Если у вас хватит терпения следовать тому, что вы наметили, — успех возможен." },
                    { 5, "Займитесь накоплением знаний, в данный момент это нужно вам больше всего." },
                    { 6, "Шаг за шагом вы приближаетесь к намеченной цели. «Тише едешь — дальше будешь» — в данном случае для вас." },
                    { 7, "Временные трудности и испытания. Сохраните достоинство и не теряйте из виду цель." },
                    { 8, "Обстоятельства сложатся удачно, добавьте смекалки или силы, чтобы убрать противостояние вашим планам." },
                    { 9, "Имейте терпение и добьетесь всего, что пожелаете. В данном случае поспешные действия неуместны.Можете рассчитывать только на плоды своих усилий. Помощь со стороны может оказаться «медвежьей услугой»." },
                    { 10, "Вы окажетесь в выигрыше. Это будет сюрпризом, так как может получиться не в то время, в какое вы предполагаете." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Predictions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Predictions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Predictions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Predictions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Predictions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Predictions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Predictions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Predictions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Predictions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Predictions",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
