using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class primedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountName = table.Column<string>(type: "text", nullable: false),
                    AccountType = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "definition_bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_definition_bank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group_People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group_Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product_Failure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Failure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Section_Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Type_People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Type_Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit_Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fund",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FirstInventory = table.Column<long>(type: "bigint", nullable: false),
                    Inventory = table.Column<long>(type: "bigint", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    NegativeBalancePolicy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fund", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fund_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "definition_bank_account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BankId = table.Column<int>(type: "integer", nullable: false),
                    account_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    type_account = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    people_account = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    card_number = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    BranchName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    BranchId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    BracnhPhone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    BranchAddres = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CardReader = table.Column<bool>(type: "boolean", nullable: false),
                    FirstInventory = table.Column<long>(type: "bigint", nullable: false),
                    Inventory = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    NegativeBalancePolicy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_definition_bank_account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_definition_bank_account_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_definition_bank_account_definition_bank_BankId",
                        column: x => x.BankId,
                        principalTable: "definition_bank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPeople = table.Column<string>(type: "text", nullable: false),
                    TypePeopleId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    CreditLimit = table.Column<decimal>(type: "numeric", nullable: false),
                    IsCreditLimit = table.Column<bool>(type: "boolean", nullable: false),
                    HowToDoBusiness = table.Column<int>(type: "integer", nullable: false),
                    OFF = table.Column<int>(type: "integer", nullable: false),
                    Business = table.Column<bool>(type: "boolean", nullable: false),
                    User = table.Column<bool>(type: "boolean", nullable: false),
                    Employee = table.Column<bool>(type: "boolean", nullable: false),
                    Investor = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TaxFree = table.Column<bool>(type: "boolean", nullable: false),
                    InitialCapital = table.Column<int>(type: "integer", nullable: false),
                    Inventory = table.Column<long>(type: "bigint", nullable: false),
                    GroupPeopleId = table.Column<int>(type: "integer", nullable: false),
                    PriceLevelID = table.Column<int>(type: "integer", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_People_Group_People_GroupPeopleId",
                        column: x => x.GroupPeopleId,
                        principalTable: "Group_People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_People_PriceLevels_PriceLevelID",
                        column: x => x.PriceLevelID,
                        principalTable: "PriceLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_People_Type_People_TypePeopleId",
                        column: x => x.TypePeopleId,
                        principalTable: "Type_People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bank_To_Bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    PeopleId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BankFirstId = table.Column<int>(type: "integer", nullable: false),
                    BankAccountFirstId = table.Column<int>(type: "integer", nullable: false),
                    IdSandFirst = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BankEndId = table.Column<int>(type: "integer", nullable: false),
                    BankAccountIdEnd = table.Column<int>(type: "integer", nullable: false),
                    BankAccountEndId = table.Column<int>(type: "integer", nullable: false),
                    IdSandEnd = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
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
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    IdSand = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FundId = table.Column<int>(type: "integer", nullable: false),
                    PeopleId = table.Column<int>(type: "integer", nullable: false),
                    BankId = table.Column<int>(type: "integer", nullable: false),
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    BankAccuntId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Storeroom_Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SectionProductId = table.Column<int>(type: "integer", nullable: false),
                    PeopleId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NegativeBalancePolicy = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storeroom_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Storeroom_Product_People_PeopleId",
                        column: x => x.PeopleId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Storeroom_Product_Section_Product_SectionProductId",
                        column: x => x.SectionProductId,
                        principalTable: "Section_Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TypeProductId = table.Column<int>(type: "integer", nullable: false),
                    UnitProductId = table.Column<int>(type: "integer", nullable: false),
                    SectionProductId = table.Column<int>(type: "integer", nullable: false),
                    StoreroomProductId = table.Column<int>(type: "integer", nullable: false),
                    GroupProductId = table.Column<int>(type: "integer", nullable: false),
                    BuyPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Profit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsTax = table.Column<bool>(type: "boolean", nullable: false),
                    Tax = table.Column<decimal>(type: "numeric", nullable: false),
                    IsWeighty = table.Column<bool>(type: "boolean", nullable: false),
                    IsIsButton = table.Column<bool>(type: "boolean", nullable: false),
                    IsBarcode = table.Column<bool>(type: "boolean", nullable: false),
                    Inventory = table.Column<int>(type: "integer", nullable: false),
                    MinInventory = table.Column<int>(type: "integer", nullable: false),
                    MaxInventory = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    ShortcutKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Group_Product_GroupProductId",
                        column: x => x.GroupProductId,
                        principalTable: "Group_Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Section_Product_SectionProductId",
                        column: x => x.SectionProductId,
                        principalTable: "Section_Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Storeroom_Product_StoreroomProductId",
                        column: x => x.StoreroomProductId,
                        principalTable: "Storeroom_Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Type_Product_TypeProductId",
                        column: x => x.TypeProductId,
                        principalTable: "Type_Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Unit_Product_UnitProductId",
                        column: x => x.UnitProductId,
                        principalTable: "Unit_Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Failure_Item",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Barcode = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Unit = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    StoreroomId = table.Column<int>(type: "integer", nullable: false),
                    Storeroom_ProductId = table.Column<int>(type: "integer", nullable: true),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProductFailureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Failure_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Failure_Item_Product_Failure_ProductFailureId",
                        column: x => x.ProductFailureId,
                        principalTable: "Product_Failure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Failure_Item_Storeroom_Product_Storeroom_ProductId",
                        column: x => x.Storeroom_ProductId,
                        principalTable: "Storeroom_Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UnitsLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    UnitProductId = table.Column<int>(type: "integer", nullable: false),
                    ConversionFactor = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitsLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitsLevel_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnitsLevel_Unit_Product_UnitProductId",
                        column: x => x.UnitProductId,
                        principalTable: "Unit_Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductBarcodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductUnitId = table.Column<int>(type: "integer", nullable: false),
                    Barcode = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBarcodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBarcodes_UnitsLevel_ProductUnitId",
                        column: x => x.ProductUnitId,
                        principalTable: "UnitsLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UnitLevelId = table.Column<int>(type: "integer", nullable: false),
                    PriceLevelId = table.Column<int>(type: "integer", nullable: false),
                    BuyPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Profit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPrices_PriceLevels_PriceLevelId",
                        column: x => x.PriceLevelId,
                        principalTable: "PriceLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductPrices_UnitsLevel_UnitLevelId",
                        column: x => x.UnitLevelId,
                        principalTable: "UnitsLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Access_Level",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupUserId = table.Column<int>(type: "integer", nullable: false),
                    IsBankT = table.Column<bool>(type: "boolean", nullable: false),
                    IsBank = table.Column<bool>(type: "boolean", nullable: false),
                    IsFund = table.Column<bool>(type: "boolean", nullable: false),
                    IsWorkShift = table.Column<bool>(type: "boolean", nullable: false),
                    IsRegisterUser = table.Column<bool>(type: "boolean", nullable: false),
                    IsInvoices = table.Column<bool>(type: "boolean", nullable: false),
                    IsPeople = table.Column<bool>(type: "boolean", nullable: false),
                    IsGroupPeople = table.Column<bool>(type: "boolean", nullable: false),
                    IsTypePeopel = table.Column<bool>(type: "boolean", nullable: false),
                    IsGroupProduct = table.Column<bool>(type: "boolean", nullable: false),
                    IsPriceLevel = table.Column<bool>(type: "boolean", nullable: false),
                    IsProductFailre = table.Column<bool>(type: "boolean", nullable: false),
                    IsProduct = table.Column<bool>(type: "boolean", nullable: false),
                    IsSectionProduct = table.Column<bool>(type: "boolean", nullable: false),
                    IsTypeProduct = table.Column<bool>(type: "boolean", nullable: false),
                    IsStoreroomProduct = table.Column<bool>(type: "boolean", nullable: false),
                    IsUnitProduct = table.Column<bool>(type: "boolean", nullable: false),
                    IsGroupUser = table.Column<bool>(type: "boolean", nullable: false),
                    IsUser = table.Column<bool>(type: "boolean", nullable: false),
                    IsAccessLevel = table.Column<bool>(type: "boolean", nullable: false),
                    IsViewingofOthers = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Access_Level", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group_User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    Access_LevelId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Group_User_Access_Level_Access_LevelId",
                        column: x => x.Access_LevelId,
                        principalTable: "Access_Level",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PeopleId = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Validity = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImageAddress = table.Column<string>(type: "text", nullable: false),
                    GroupUserId = table.Column<int>(type: "integer", nullable: false),
                    CurrentSessionId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Group_User_GroupUserId",
                        column: x => x.GroupUserId,
                        principalTable: "Group_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_People_PeopleId",
                        column: x => x.PeopleId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cash_Register_To_The_User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FundId = table.Column<int>(type: "integer", nullable: false),
                    InitialAmount = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cash_Register_To_The_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cash_Register_To_The_User_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cash_Register_To_The_User_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeopleId = table.Column<int>(type: "integer", nullable: false),
                    NumberofAllItems = table.Column<int>(type: "integer", nullable: false),
                    OffAll = table.Column<int>(type: "integer", nullable: false),
                    TotalSum = table.Column<int>(type: "integer", nullable: false),
                    IsUpdate = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TyepPay = table.Column<int>(type: "integer", nullable: false),
                    TypeInvoices = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_People_PeopleId",
                        column: x => x.PeopleId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reminder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reminder_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Work_Shift",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CashRegisterToUserId = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OpeningAmount = table.Column<long>(type: "bigint", nullable: false),
                    ClosingAmount = table.Column<long>(type: "bigint", nullable: false),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Shift", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Work_Shift_Cash_Register_To_The_User_CashRegisterToUserId",
                        column: x => x.CashRegisterToUserId,
                        principalTable: "Cash_Register_To_The_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices_Item",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Barcode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    OFF = table.Column<int>(type: "integer", nullable: false),
                    AllPrice = table.Column<int>(type: "integer", nullable: false),
                    InvoicesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Item_Invoices_InvoicesId",
                        column: x => x.InvoicesId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Item_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    RelatedDocumentType = table.Column<string>(type: "text", nullable: false),
                    RelatedDocumentId = table.Column<int>(type: "integer", nullable: true),
                    PaymentMethod = table.Column<string>(type: "text", nullable: true),
                    RelatedAccountId = table.Column<int>(type: "integer", nullable: true),
                    WorkShiftId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Work_Shift_WorkShiftId",
                        column: x => x.WorkShiftId,
                        principalTable: "Work_Shift",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Access_Level_GroupUserId",
                table: "Access_Level",
                column: "GroupUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_AccountName",
                table: "Account",
                column: "AccountName",
                unique: true);

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
                name: "IX_Cash_Register_To_The_User_FundId",
                table: "Cash_Register_To_The_User",
                column: "FundId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cash_Register_To_The_User_UserId",
                table: "Cash_Register_To_The_User",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_definition_bank_account_AccountId",
                table: "definition_bank_account",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_definition_bank_account_BankId",
                table: "definition_bank_account",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Fund_AccountId",
                table: "Fund",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_User_Access_LevelId",
                table: "Group_User",
                column: "Access_LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PeopleId",
                table: "Invoices",
                column: "PeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UserId",
                table: "Invoices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Item_InvoicesId",
                table: "Invoices_Item",
                column: "InvoicesId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Item_ProductId",
                table: "Invoices_Item",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LogUser_UserId",
                table: "LogUser",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_People_AccountId",
                table: "People",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_People_GroupPeopleId",
                table: "People",
                column: "GroupPeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_People_PriceLevelID",
                table: "People",
                column: "PriceLevelID");

            migrationBuilder.CreateIndex(
                name: "IX_People_TypePeopleId",
                table: "People",
                column: "TypePeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_GroupProductId",
                table: "Product",
                column: "GroupProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SectionProductId",
                table: "Product",
                column: "SectionProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_StoreroomProductId",
                table: "Product",
                column: "StoreroomProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_TypeProductId",
                table: "Product",
                column: "TypeProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_UnitProductId",
                table: "Product",
                column: "UnitProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Failure_Item_ProductFailureId",
                table: "Product_Failure_Item",
                column: "ProductFailureId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Failure_Item_Storeroom_ProductId",
                table: "Product_Failure_Item",
                column: "Storeroom_ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBarcodes_ProductUnitId",
                table: "ProductBarcodes",
                column: "ProductUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_PriceLevelId",
                table: "ProductPrices",
                column: "PriceLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_UnitLevelId",
                table: "ProductPrices",
                column: "UnitLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_UserId",
                table: "Reminder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Storeroom_Product_PeopleId",
                table: "Storeroom_Product",
                column: "PeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_Storeroom_Product_SectionProductId",
                table: "Storeroom_Product",
                column: "SectionProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_WorkShiftId",
                table: "Transaction",
                column: "WorkShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitsLevel_ProductId",
                table: "UnitsLevel",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitsLevel_UnitProductId",
                table: "UnitsLevel",
                column: "UnitProductId");

            migrationBuilder.CreateIndex(
                name: "IX_User_GroupUserId",
                table: "User",
                column: "GroupUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_PeopleId",
                table: "User",
                column: "PeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Work_Shift_CashRegisterToUserId",
                table: "Work_Shift",
                column: "CashRegisterToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Access_Level_Group_User_GroupUserId",
                table: "Access_Level",
                column: "GroupUserId",
                principalTable: "Group_User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Access_Level_Group_User_GroupUserId",
                table: "Access_Level");

            migrationBuilder.DropTable(
                name: "Bank_To_Bank");

            migrationBuilder.DropTable(
                name: "Invoices_Item");

            migrationBuilder.DropTable(
                name: "LogUser");

            migrationBuilder.DropTable(
                name: "Pay_To_Bank");

            migrationBuilder.DropTable(
                name: "Product_Failure_Item");

            migrationBuilder.DropTable(
                name: "ProductBarcodes");

            migrationBuilder.DropTable(
                name: "ProductPrices");

            migrationBuilder.DropTable(
                name: "Reminder");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "definition_bank_account");

            migrationBuilder.DropTable(
                name: "Product_Failure");

            migrationBuilder.DropTable(
                name: "UnitsLevel");

            migrationBuilder.DropTable(
                name: "Work_Shift");

            migrationBuilder.DropTable(
                name: "definition_bank");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Cash_Register_To_The_User");

            migrationBuilder.DropTable(
                name: "Group_Product");

            migrationBuilder.DropTable(
                name: "Storeroom_Product");

            migrationBuilder.DropTable(
                name: "Type_Product");

            migrationBuilder.DropTable(
                name: "Unit_Product");

            migrationBuilder.DropTable(
                name: "Fund");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Section_Product");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Group_People");

            migrationBuilder.DropTable(
                name: "PriceLevels");

            migrationBuilder.DropTable(
                name: "Type_People");

            migrationBuilder.DropTable(
                name: "Group_User");

            migrationBuilder.DropTable(
                name: "Access_Level");
        }
    }
}
