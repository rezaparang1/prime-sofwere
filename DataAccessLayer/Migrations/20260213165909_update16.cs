using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class update16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubDiscountProduct_Product_ProductId",
                table: "ClubDiscountProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerLevel_CustomerLevelId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTransaction_Invoices_InvoiceId",
                table: "PointTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicDiscountProduct_Product_ProductId",
                table: "PublicDiscountProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransaction_ClubDiscount_ClubDiscountId",
                table: "WalletTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransaction_Invoices_InvoiceId",
                table: "WalletTransaction");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WalletTransaction",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "WalletTransaction",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Wallet",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "PublicDiscountProduct",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "OriginalPrice",
                table: "PublicDiscountProduct",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountedPrice",
                table: "PublicDiscountProduct",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "UnitLevelId",
                table: "PublicDiscountProduct",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "PublicDiscount",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "PublicDiscount",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PublicDiscount",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PointTransaction",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Invoices",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CustomerLevel",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CustomerLevel",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPurchaseAmount",
                table: "Customer",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<int>(
                name: "PeopleId",
                table: "Customer",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Customer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ClubDiscountProduct",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "OriginalPrice",
                table: "ClubDiscountProduct",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "ClubPrice",
                table: "ClubDiscountProduct",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "UnitLevelId",
                table: "ClubDiscountProduct",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "ClubDiscount",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ClubDiscount",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ClubDiscount",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ManagerName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicDiscountProduct_UnitLevelId",
                table: "PublicDiscountProduct",
                column: "UnitLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicDiscount_StoreId",
                table: "PublicDiscount",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLevel_StoreId",
                table: "CustomerLevel",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Barcode",
                table: "Customer",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Email",
                table: "Customer",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Mobile",
                table: "Customer",
                column: "Mobile",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_PeopleId",
                table: "Customer",
                column: "PeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_StoreId",
                table: "Customer",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubDiscountProduct_UnitLevelId",
                table: "ClubDiscountProduct",
                column: "UnitLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubDiscount_StoreId",
                table: "ClubDiscount",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubDiscount_Store_StoreId",
                table: "ClubDiscount",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubDiscountProduct_Product_ProductId",
                table: "ClubDiscountProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubDiscountProduct_UnitsLevel_UnitLevelId",
                table: "ClubDiscountProduct",
                column: "UnitLevelId",
                principalTable: "UnitsLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerLevel_CustomerLevelId",
                table: "Customer",
                column: "CustomerLevelId",
                principalTable: "CustomerLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_People_PeopleId",
                table: "Customer",
                column: "PeopleId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Store_StoreId",
                table: "Customer",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerLevel_Store_StoreId",
                table: "CustomerLevel",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PointTransaction_Invoices_InvoiceId",
                table: "PointTransaction",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicDiscount_Store_StoreId",
                table: "PublicDiscount",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicDiscountProduct_Product_ProductId",
                table: "PublicDiscountProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicDiscountProduct_UnitsLevel_UnitLevelId",
                table: "PublicDiscountProduct",
                column: "UnitLevelId",
                principalTable: "UnitsLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransaction_ClubDiscount_ClubDiscountId",
                table: "WalletTransaction",
                column: "ClubDiscountId",
                principalTable: "ClubDiscount",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransaction_Invoices_InvoiceId",
                table: "WalletTransaction",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubDiscount_Store_StoreId",
                table: "ClubDiscount");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubDiscountProduct_Product_ProductId",
                table: "ClubDiscountProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubDiscountProduct_UnitsLevel_UnitLevelId",
                table: "ClubDiscountProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerLevel_CustomerLevelId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_People_PeopleId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Store_StoreId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerLevel_Store_StoreId",
                table: "CustomerLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_PointTransaction_Invoices_InvoiceId",
                table: "PointTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicDiscount_Store_StoreId",
                table: "PublicDiscount");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicDiscountProduct_Product_ProductId",
                table: "PublicDiscountProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicDiscountProduct_UnitsLevel_UnitLevelId",
                table: "PublicDiscountProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransaction_ClubDiscount_ClubDiscountId",
                table: "WalletTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransaction_Invoices_InvoiceId",
                table: "WalletTransaction");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropIndex(
                name: "IX_PublicDiscountProduct_UnitLevelId",
                table: "PublicDiscountProduct");

            migrationBuilder.DropIndex(
                name: "IX_PublicDiscount_StoreId",
                table: "PublicDiscount");

            migrationBuilder.DropIndex(
                name: "IX_CustomerLevel_StoreId",
                table: "CustomerLevel");

            migrationBuilder.DropIndex(
                name: "IX_Customer_Barcode",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_Email",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_Mobile",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_PeopleId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_StoreId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_ClubDiscountProduct_UnitLevelId",
                table: "ClubDiscountProduct");

            migrationBuilder.DropIndex(
                name: "IX_ClubDiscount_StoreId",
                table: "ClubDiscount");

            migrationBuilder.DropColumn(
                name: "UnitLevelId",
                table: "PublicDiscountProduct");

            migrationBuilder.DropColumn(
                name: "PeopleId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "UnitLevelId",
                table: "ClubDiscountProduct");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WalletTransaction",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "WalletTransaction",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Wallet",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "PublicDiscountProduct",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OriginalPrice",
                table: "PublicDiscountProduct",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "DiscountedPrice",
                table: "PublicDiscountProduct",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "PublicDiscount",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "PublicDiscount",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PublicDiscount",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PointTransaction",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CustomerLevel",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CustomerLevel",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPurchaseAmount",
                table: "Customer",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ClubDiscountProduct",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OriginalPrice",
                table: "ClubDiscountProduct",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "ClubPrice",
                table: "ClubDiscountProduct",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "ClubDiscount",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ClubDiscount",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ClubDiscount",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubDiscountProduct_Product_ProductId",
                table: "ClubDiscountProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerLevel_CustomerLevelId",
                table: "Customer",
                column: "CustomerLevelId",
                principalTable: "CustomerLevel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PointTransaction_Invoices_InvoiceId",
                table: "PointTransaction",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicDiscountProduct_Product_ProductId",
                table: "PublicDiscountProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransaction_ClubDiscount_ClubDiscountId",
                table: "WalletTransaction",
                column: "ClubDiscountId",
                principalTable: "ClubDiscount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransaction_Invoices_InvoiceId",
                table: "WalletTransaction",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id");
        }
    }
}
