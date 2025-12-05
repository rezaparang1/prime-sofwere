using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Work_Shift_Cash_Register_To_The_User_CashRegisterToUserId",
                table: "Work_Shift");

            migrationBuilder.DropIndex(
                name: "IX_Cash_Register_To_The_User_FundId",
                table: "Cash_Register_To_The_User");

            migrationBuilder.DropIndex(
                name: "IX_Cash_Register_To_The_User_UserId",
                table: "Cash_Register_To_The_User");

            migrationBuilder.AddColumn<bool>(
                name: "IsAuto",
                table: "Work_Shift",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivity",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Cash_Register_To_The_User_FundId",
                table: "Cash_Register_To_The_User",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_Cash_Register_To_The_User_UserId",
                table: "Cash_Register_To_The_User",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Work_Shift_Cash_Register_To_The_User_CashRegisterToUserId",
                table: "Work_Shift",
                column: "CashRegisterToUserId",
                principalTable: "Cash_Register_To_The_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Work_Shift_Cash_Register_To_The_User_CashRegisterToUserId",
                table: "Work_Shift");

            migrationBuilder.DropIndex(
                name: "IX_Cash_Register_To_The_User_FundId",
                table: "Cash_Register_To_The_User");

            migrationBuilder.DropIndex(
                name: "IX_Cash_Register_To_The_User_UserId",
                table: "Cash_Register_To_The_User");

            migrationBuilder.DropColumn(
                name: "IsAuto",
                table: "Work_Shift");

            migrationBuilder.DropColumn(
                name: "LastActivity",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_Cash_Register_To_The_User_FundId",
                table: "Cash_Register_To_The_User",
                column: "FundId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cash_Register_To_The_User_UserId",
                table: "Cash_Register_To_The_User",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Work_Shift_Cash_Register_To_The_User_CashRegisterToUserId",
                table: "Work_Shift",
                column: "CashRegisterToUserId",
                principalTable: "Cash_Register_To_The_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
