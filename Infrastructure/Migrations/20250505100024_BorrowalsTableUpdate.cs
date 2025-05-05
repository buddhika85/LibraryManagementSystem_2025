using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BorrowalsTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowals_AspNetUsers_MemberId1",
                table: "Borrowals");

            migrationBuilder.DropIndex(
                name: "IX_Borrowals_MemberId1",
                table: "Borrowals");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Borrowals");

            migrationBuilder.DropColumn(
                name: "MemberId1",
                table: "Borrowals");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Borrowals",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowals_AppUserId",
                table: "Borrowals",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowals_AspNetUsers_AppUserId",
                table: "Borrowals",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowals_AspNetUsers_AppUserId",
                table: "Borrowals");

            migrationBuilder.DropIndex(
                name: "IX_Borrowals_AppUserId",
                table: "Borrowals");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Borrowals");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Borrowals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MemberId1",
                table: "Borrowals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borrowals_MemberId1",
                table: "Borrowals",
                column: "MemberId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowals_AspNetUsers_MemberId1",
                table: "Borrowals",
                column: "MemberId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
