using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BorrowalReturnsTableV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LmsOwnerGivenMoney",
                table: "BorrowalReturns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PerDayLateFeeDollars",
                table: "BorrowalReturns",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "lateDays",
                table: "BorrowalReturns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LmsOwnerGivenMoney",
                table: "BorrowalReturns");

            migrationBuilder.DropColumn(
                name: "PerDayLateFeeDollars",
                table: "BorrowalReturns");

            migrationBuilder.DropColumn(
                name: "lateDays",
                table: "BorrowalReturns");
        }
    }
}
