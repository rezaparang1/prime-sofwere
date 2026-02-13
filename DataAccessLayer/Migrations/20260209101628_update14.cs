using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class update14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_PriceLevels_PriceLevelsId",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "Bank_To_Bank");

            migrationBuilder.DropTable(
                name: "Pay_To_Bank");

            migrationBuilder.DropIndex(
                name: "IX_Customer_PriceLevelsId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Britday",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "PriceLevelId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "PriceLevelsId",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Customer",
                newName: "TotalPoints");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Customer",
                newName: "IsActive");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Customer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "WalletBalance",
                table: "Customer",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "WalletBalance",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "TotalPoints",
                table: "Customer",
                newName: "Score");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Customer",
                newName: "IsDelete");

            migrationBuilder.AddColumn<DateTime>(
                name: "Britday",
                table: "Customer",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PriceLevelId",
                table: "Customer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PriceLevelsId",
                table: "Customer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Bank_To_Bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BankAccountEndId = table.Column<int>(type: "integer", nullable: false),
                    BankAccountFirstId = table.Column<int>(type: "integer", nullable: false),
                    BankEndId = table.Column<int>(type: "integer", nullable: false),
                    BankFirstId = table.Column<int>(type: "integer", nullable: false),
                    PeopleId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    BankAccountIdEnd = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IdSandEnd = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IdSandFirst = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank_To_Bank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bank_To_Bank_People_PeopleId",
                        column: x => x.PeopleId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bank_To_Bank_definition_bank_BankEndId",
                        column: x => x.BankEndId,
                        principalTable: "definition_bank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bank_To_Bank_definition_bank_BankFirstId",
                        column: x => x.BankFirstId,
                        principalTable: "definition_bank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bank_To_Bank_definition_bank_account_BankAccountEndId",
                        column: x => x.BankAccountEndId,
                        principalTable: "definition_bank_account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bank_To_Bank_definition_bank_account_BankAccountFirstId",
                        column: x => x.BankAccountFirstId,
                        principalTable: "definition_bank_account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pay_To_Bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BankAccuntId = table.Column<int>(type: "integer", nullable: true),
                    BankId = table.Column<int>(type: "integer", nullable: false),
                    FundId = table.Column<int>(type: "integer", nullable: false),
                    PeopleId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IdSand = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pay_To_Bank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pay_To_Bank_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pay_To_Bank_People_PeopleId",
                        column: x => x.PeopleId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pay_To_Bank_definition_bank_BankId",
                        column: x => x.BankId,
                        principalTable: "definition_bank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pay_To_Bank_definition_bank_account_BankAccuntId",
                        column: x => x.BankAccuntId,
                        principalTable: "definition_bank_account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_PriceLevelsId",
                table: "Customer",
                column: "PriceLevelsId");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_To_Bank_BankAccountEndId",
                table: "Bank_To_Bank",
                column: "BankAccountEndId");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_To_Bank_BankAccountFirstId",
                table: "Bank_To_Bank",
                column: "BankAccountFirstId");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_To_Bank_BankEndId",
                table: "Bank_To_Bank",
                column: "BankEndId");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_To_Bank_BankFirstId",
                table: "Bank_To_Bank",
                column: "BankFirstId");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_To_Bank_PeopleId",
                table: "Bank_To_Bank",
                column: "PeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_Pay_To_Bank_BankAccuntId",
                table: "Pay_To_Bank",
                column: "BankAccuntId");

            migrationBuilder.CreateIndex(
                name: "IX_Pay_To_Bank_BankId",
                table: "Pay_To_Bank",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Pay_To_Bank_FundId",
                table: "Pay_To_Bank",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_Pay_To_Bank_PeopleId",
                table: "Pay_To_Bank",
                column: "PeopleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_PriceLevels_PriceLevelsId",
                table: "Customer",
                column: "PriceLevelsId",
                principalTable: "PriceLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
