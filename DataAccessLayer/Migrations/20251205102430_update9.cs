using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class update9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Product_Failure_Item",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_Failure_Item_ProductId",
                table: "Product_Failure_Item",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Failure_Item_Product_ProductId",
                table: "Product_Failure_Item",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Failure_Item_Product_ProductId",
                table: "Product_Failure_Item");

            migrationBuilder.DropIndex(
                name: "IX_Product_Failure_Item_ProductId",
                table: "Product_Failure_Item");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Product_Failure_Item");
        }
    }
}
