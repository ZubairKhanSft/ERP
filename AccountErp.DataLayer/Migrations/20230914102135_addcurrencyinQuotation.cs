using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountErp.DataLayer.Migrations
{
    public partial class addcurrencyinQuotation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ConversionRate",
                table: "Quotations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Quotations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalAmountAfterConversion",
                table: "Quotations",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversionRate",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "TotalAmountAfterConversion",
                table: "Quotations");
        }
    }
}
