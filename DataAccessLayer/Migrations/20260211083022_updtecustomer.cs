using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updtecustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Family",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "WalletBalance",
                table: "Customer",
                newName: "TotalPurchaseAmount");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Customer",
                newName: "Mobile");

            migrationBuilder.AddColumn<int>(
                name: "ClubDiscount",
                table: "Invoices_Item",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClubDiscountId",
                table: "Invoices_Item",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginalPrice",
                table: "Invoices_Item",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PublicDiscountId",
                table: "Invoices_Item",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClubDiscountAll",
                table: "Invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerLevelId",
                table: "Invoices",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EarnedPoints",
                table: "Invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsClubDiscountRefunded",
                table: "Invoices",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LevelDiscountAmount",
                table: "Invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsedWalletAmount",
                table: "Invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Customer",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "CurrentPoints",
                table: "Customer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerLevelId",
                table: "Customer",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Customer",
                type: "character varying(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Customer",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsClubMember",
                table: "Customer",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Customer",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPurchaseDate",
                table: "Customer",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterDate",
                table: "Customer",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TotalPurchaseCount",
                table: "Customer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClubDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    RefundToWallet = table.Column<bool>(type: "boolean", nullable: false),
                    StoreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubDiscount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MinPoints = table.Column<int>(type: "integer", nullable: false),
                    MaxPoints = table.Column<int>(type: "integer", nullable: true),
                    DiscountPercent = table.Column<int>(type: "integer", nullable: false),
                    StoreId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PointTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    InvoiceId = table.Column<int>(type: "integer", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointTransaction_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointTransaction_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PublicDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    StoreId = table.Column<int>(type: "integer", nullable: false),
                    Monday = table.Column<bool>(type: "boolean", nullable: false),
                    Tuesday = table.Column<bool>(type: "boolean", nullable: false),
                    Wednesday = table.Column<bool>(type: "boolean", nullable: false),
                    Thursday = table.Column<bool>(type: "boolean", nullable: false),
                    Friday = table.Column<bool>(type: "boolean", nullable: false),
                    Saturday = table.Column<bool>(type: "boolean", nullable: false),
                    Sunday = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicDiscount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    Balance = table.Column<int>(type: "integer", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallet_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClubDiscountProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClubDiscountId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    ClubPrice = table.Column<int>(type: "integer", nullable: false),
                    OriginalPrice = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubDiscountProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClubDiscountProduct_ClubDiscount_ClubDiscountId",
                        column: x => x.ClubDiscountId,
                        principalTable: "ClubDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubDiscountProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerLevelHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    CustomerLevelId = table.Column<int>(type: "integer", nullable: false),
                    FromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLevelHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerLevelHistory_CustomerLevel_CustomerLevelId",
                        column: x => x.CustomerLevelId,
                        principalTable: "CustomerLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerLevelHistory_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublicDiscountProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicDiscountId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    DiscountedPrice = table.Column<int>(type: "integer", nullable: false),
                    OriginalPrice = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicDiscountProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicDiscountProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicDiscountProduct_PublicDiscount_PublicDiscountId",
                        column: x => x.PublicDiscountId,
                        principalTable: "PublicDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WalletId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    InvoiceId = table.Column<int>(type: "integer", nullable: true),
                    ClubDiscountId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletTransaction_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WalletTransaction_Wallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Item_ClubDiscountId",
                table: "Invoices_Item",
                column: "ClubDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Item_PublicDiscountId",
                table: "Invoices_Item",
                column: "PublicDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerLevelId",
                table: "Invoices",
                column: "CustomerLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerLevelId",
                table: "Customer",
                column: "CustomerLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubDiscountProduct_ClubDiscountId",
                table: "ClubDiscountProduct",
                column: "ClubDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubDiscountProduct_ProductId",
                table: "ClubDiscountProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLevelHistory_CustomerId",
                table: "CustomerLevelHistory",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLevelHistory_CustomerLevelId",
                table: "CustomerLevelHistory",
                column: "CustomerLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransaction_CustomerId",
                table: "PointTransaction",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransaction_InvoiceId",
                table: "PointTransaction",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicDiscountProduct_ProductId",
                table: "PublicDiscountProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicDiscountProduct_PublicDiscountId",
                table: "PublicDiscountProduct",
                column: "PublicDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_CustomerId",
                table: "Wallet",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransaction_InvoiceId",
                table: "WalletTransaction",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransaction_WalletId",
                table: "WalletTransaction",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerLevel_CustomerLevelId",
                table: "Customer",
                column: "CustomerLevelId",
                principalTable: "CustomerLevel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_CustomerLevel_CustomerLevelId",
                table: "Invoices",
                column: "CustomerLevelId",
                principalTable: "CustomerLevel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Item_ClubDiscount_ClubDiscountId",
                table: "Invoices_Item",
                column: "ClubDiscountId",
                principalTable: "ClubDiscount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Item_PublicDiscount_PublicDiscountId",
                table: "Invoices_Item",
                column: "PublicDiscountId",
                principalTable: "PublicDiscount",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerLevel_CustomerLevelId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_CustomerLevel_CustomerLevelId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Item_ClubDiscount_ClubDiscountId",
                table: "Invoices_Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Item_PublicDiscount_PublicDiscountId",
                table: "Invoices_Item");

            migrationBuilder.DropTable(
                name: "ClubDiscountProduct");

            migrationBuilder.DropTable(
                name: "CustomerLevelHistory");

            migrationBuilder.DropTable(
                name: "PointTransaction");

            migrationBuilder.DropTable(
                name: "PublicDiscountProduct");

            migrationBuilder.DropTable(
                name: "WalletTransaction");

            migrationBuilder.DropTable(
                name: "ClubDiscount");

            migrationBuilder.DropTable(
                name: "CustomerLevel");

            migrationBuilder.DropTable(
                name: "PublicDiscount");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_Item_ClubDiscountId",
                table: "Invoices_Item");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_Item_PublicDiscountId",
                table: "Invoices_Item");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CustomerLevelId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Customer_CustomerLevelId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ClubDiscount",
                table: "Invoices_Item");

            migrationBuilder.DropColumn(
                name: "ClubDiscountId",
                table: "Invoices_Item");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Invoices_Item");

            migrationBuilder.DropColumn(
                name: "PublicDiscountId",
                table: "Invoices_Item");

            migrationBuilder.DropColumn(
                name: "ClubDiscountAll",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CustomerLevelId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "EarnedPoints",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "IsClubDiscountRefunded",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "LevelDiscountAmount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "UsedWalletAmount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CurrentPoints",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CustomerLevelId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "IsClubMember",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LastPurchaseDate",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "RegisterDate",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "TotalPurchaseCount",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "TotalPurchaseAmount",
                table: "Customer",
                newName: "WalletBalance");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "Customer",
                newName: "Phone");

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Customer",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Family",
                table: "Customer",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Customer",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");
        }
    }
}
