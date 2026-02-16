using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class fixacceslevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsWorkShift",
                table: "Access_Level",
                newName: "IsWalletIncreaseandDecrease");

            migrationBuilder.RenameColumn(
                name: "IsUnitProduct",
                table: "Access_Level",
                newName: "IsSystemlog");

            migrationBuilder.RenameColumn(
                name: "IsTypeProduct",
                table: "Access_Level",
                newName: "IsShiftWork");

            migrationBuilder.RenameColumn(
                name: "IsTypePeopel",
                table: "Access_Level",
                newName: "IsSentMessagesR");

            migrationBuilder.RenameColumn(
                name: "IsSectionProduct",
                table: "Access_Level",
                newName: "IsSendSMS");

            migrationBuilder.RenameColumn(
                name: "IsRegisterUser",
                table: "Access_Level",
                newName: "IsSalesRInvoiceR");

            migrationBuilder.RenameColumn(
                name: "IsProductFailre",
                table: "Access_Level",
                newName: "IsSalesRInvoice");

            migrationBuilder.RenameColumn(
                name: "IsProduct",
                table: "Access_Level",
                newName: "IsSalesPeopleR");

            migrationBuilder.RenameColumn(
                name: "IsPriceLevel",
                table: "Access_Level",
                newName: "IsSalesInvoiceR");

            migrationBuilder.RenameColumn(
                name: "IsPeople",
                table: "Access_Level",
                newName: "IsSalesInvoice");

            migrationBuilder.RenameColumn(
                name: "IsInvoices",
                table: "Access_Level",
                newName: "IsSMSLogR");

            migrationBuilder.RenameColumn(
                name: "IsGroupProduct",
                table: "Access_Level",
                newName: "IsSMSDefinition");

            migrationBuilder.RenameColumn(
                name: "IsGroupPeople",
                table: "Access_Level",
                newName: "IsRecovery");

            migrationBuilder.RenameColumn(
                name: "IsBankT",
                table: "Access_Level",
                newName: "IsQuarterlyProductR");

            migrationBuilder.RenameColumn(
                name: "IsBank",
                table: "Access_Level",
                newName: "IsPurchaseRInvoiceR");

            migrationBuilder.RenameColumn(
                name: "IsAccessLevel",
                table: "Access_Level",
                newName: "IsPurchaseRInvoice");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccessLevelDefinition",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBackup",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBankAccountDefinition",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBankDetailedR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBankandFundInventory",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBasicData",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCardTransactionR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsClubDiscountDefinition",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsClubDiscountReport",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomerDefinition",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomerDetailR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomerLevelDefinition",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomerLevelReport",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomerReport",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDailySalesR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefinitionPeople",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefinitionProduct",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeliverytoUser",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDetailedProductDiscountR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFundDetailedR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInventoryProduct",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsKardexProductR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMonthlyProductR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOerder",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOerderR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOrderPointR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPeopleR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProductFailure",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProductFailureR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProductTransactionR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublicDiscount",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPurchaseInvoice",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPurchaseInvoiceR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPurchasePeopleR",
                table: "Access_Level",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccessLevelDefinition",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsBackup",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsBankAccountDefinition",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsBankDetailedR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsBankandFundInventory",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsBasicData",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsCardTransactionR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsClubDiscountDefinition",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsClubDiscountReport",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsCustomerDefinition",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsCustomerDetailR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsCustomerLevelDefinition",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsCustomerLevelReport",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsCustomerReport",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsDailySalesR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsDefinitionPeople",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsDefinitionProduct",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsDeliverytoUser",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsDetailedProductDiscountR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsFundDetailedR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsInventoryProduct",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsKardexProductR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsMonthlyProductR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsOerder",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsOerderR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsOrderPointR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsPeopleR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsProductFailure",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsProductFailureR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsProductTransactionR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsPublicDiscount",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsPurchaseInvoice",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsPurchaseInvoiceR",
                table: "Access_Level");

            migrationBuilder.DropColumn(
                name: "IsPurchasePeopleR",
                table: "Access_Level");

            migrationBuilder.RenameColumn(
                name: "IsWalletIncreaseandDecrease",
                table: "Access_Level",
                newName: "IsWorkShift");

            migrationBuilder.RenameColumn(
                name: "IsSystemlog",
                table: "Access_Level",
                newName: "IsUnitProduct");

            migrationBuilder.RenameColumn(
                name: "IsShiftWork",
                table: "Access_Level",
                newName: "IsTypeProduct");

            migrationBuilder.RenameColumn(
                name: "IsSentMessagesR",
                table: "Access_Level",
                newName: "IsTypePeopel");

            migrationBuilder.RenameColumn(
                name: "IsSendSMS",
                table: "Access_Level",
                newName: "IsSectionProduct");

            migrationBuilder.RenameColumn(
                name: "IsSalesRInvoiceR",
                table: "Access_Level",
                newName: "IsRegisterUser");

            migrationBuilder.RenameColumn(
                name: "IsSalesRInvoice",
                table: "Access_Level",
                newName: "IsProductFailre");

            migrationBuilder.RenameColumn(
                name: "IsSalesPeopleR",
                table: "Access_Level",
                newName: "IsProduct");

            migrationBuilder.RenameColumn(
                name: "IsSalesInvoiceR",
                table: "Access_Level",
                newName: "IsPriceLevel");

            migrationBuilder.RenameColumn(
                name: "IsSalesInvoice",
                table: "Access_Level",
                newName: "IsPeople");

            migrationBuilder.RenameColumn(
                name: "IsSMSLogR",
                table: "Access_Level",
                newName: "IsInvoices");

            migrationBuilder.RenameColumn(
                name: "IsSMSDefinition",
                table: "Access_Level",
                newName: "IsGroupProduct");

            migrationBuilder.RenameColumn(
                name: "IsRecovery",
                table: "Access_Level",
                newName: "IsGroupPeople");

            migrationBuilder.RenameColumn(
                name: "IsQuarterlyProductR",
                table: "Access_Level",
                newName: "IsBankT");

            migrationBuilder.RenameColumn(
                name: "IsPurchaseRInvoiceR",
                table: "Access_Level",
                newName: "IsBank");

            migrationBuilder.RenameColumn(
                name: "IsPurchaseRInvoice",
                table: "Access_Level",
                newName: "IsAccessLevel");
        }
    }
}
