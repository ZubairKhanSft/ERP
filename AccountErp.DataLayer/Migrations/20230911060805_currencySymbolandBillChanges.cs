using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountErp.DataLayer.Migrations
{
    public partial class currencySymbolandBillChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "CurrencyCr",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "ConversionRate",
                table: "Bills",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Bills",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalAmountAfterConversion",
                table: "Bills",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "CurrencyCr");

            migrationBuilder.DropColumn(
                name: "ConversionRate",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "TotalAmountAfterConversion",
                table: "Bills");
        }
    }
}
