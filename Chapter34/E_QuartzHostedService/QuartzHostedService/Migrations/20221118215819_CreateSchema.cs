using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuartzHostedService.Migrations
{
    /// <inheritdoc />
    public partial class CreateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    ExchangeRatesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Base = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.ExchangeRatesId);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRateValues",
                columns: table => new
                {
                    ExchangeRateValuesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rate = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExchangeRatesId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRateValues", x => x.ExchangeRateValuesId);
                    table.ForeignKey(
                        name: "FK_ExchangeRateValues_ExchangeRates_ExchangeRatesId",
                        column: x => x.ExchangeRatesId,
                        principalTable: "ExchangeRates",
                        principalColumn: "ExchangeRatesId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRateValues_ExchangeRatesId",
                table: "ExchangeRateValues",
                column: "ExchangeRatesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeRateValues");

            migrationBuilder.DropTable(
                name: "ExchangeRates");
        }
    }
}
