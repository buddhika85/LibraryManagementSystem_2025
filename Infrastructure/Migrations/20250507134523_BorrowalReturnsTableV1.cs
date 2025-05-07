using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BorrowalReturnsTableV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BorrowalReturns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WasOverdue = table.Column<bool>(type: "bit", nullable: false),
                    WasPaid = table.Column<bool>(type: "bit", nullable: false),
                    AmountAccepted = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BorrowalsId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowalReturns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowalReturns_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BorrowalReturns_Borrowals_BorrowalsId",
                        column: x => x.BorrowalsId,
                        principalTable: "Borrowals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowalReturns_AppUserId",
                table: "BorrowalReturns",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowalReturns_BorrowalsId",
                table: "BorrowalReturns",
                column: "BorrowalsId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowalReturns");
        }
    }
}
