using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class fixend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "WalletTransaction",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Wallet",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "PublicDiscount",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RecurringDays",
                table: "PublicDiscount",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransaction_ClubDiscountId",
                table: "WalletTransaction",
                column: "ClubDiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransaction_ClubDiscount_ClubDiscountId",
                table: "WalletTransaction",
                column: "ClubDiscountId",
                principalTable: "ClubDiscount",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransaction_ClubDiscount_ClubDiscountId",
                table: "WalletTransaction");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransaction_ClubDiscountId",
                table: "WalletTransaction");

            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "PublicDiscount");

            migrationBuilder.DropColumn(
                name: "RecurringDays",
                table: "PublicDiscount");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "WalletTransaction",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<int>(
                name: "Balance",
                table: "Wallet",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
