using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Arrba.Domain.Migrations
{
    public partial class InitialArrbaDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 120, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WatchWeightStatus = table.Column<int>(nullable: false),
                    LikeValue = table.Column<long>(nullable: false),
                    LikeCount = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CategGroups",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategGroups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CheckBoxGroups",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NameMultiLangJson = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckBoxGroups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AdVehicleID = table.Column<long>(nullable: false),
                    CommentParentID = table.Column<long>(nullable: true),
                    UserID = table.Column<long>(nullable: false),
                    ForUserID = table.Column<long>(nullable: false),
                    OfferAdID = table.Column<long>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(maxLength: 2048, nullable: true),
                    ClaimCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_CommentParentID",
                        column: x => x.CommentParentID,
                        principalTable: "Comments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NameMultiLangJson = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Alias = table.Column<string>(nullable: true),
                    FirstDomainName = table.Column<string>(maxLength: 6, nullable: true),
                    ActiveStatus = table.Column<int>(nullable: false),
                    UseNativeCurrencyOnly = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CountryID = table.Column<long>(nullable: false),
                    Code = table.Column<int>(nullable: false),
                    Symbol = table.Column<string>(maxLength: 3, nullable: true),
                    Name = table.Column<string>(maxLength: 3, nullable: true),
                    FullName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FeedBacks",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(nullable: true),
                    Text = table.Column<string>(maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBacks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PropertyGroups",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NameMultiLangJson = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyGroups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServicePrices",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    SevicePriceName = table.Column<string>(maxLength: 20, nullable: true),
                    Price = table.Column<double>(nullable: false),
                    ServiceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePrices", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SuperCategories",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NameMultiLangJson = table.Column<string>(nullable: true),
                    Alias = table.Column<string>(maxLength: 180, nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    SuperCategType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperCategories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NameMultiLangJson = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WatchWeightStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    UserNickName = table.Column<string>(nullable: true),
                    UserLastName = table.Column<string>(nullable: true),
                    AvatarImgName = table.Column<string>(maxLength: 35, nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    LastLogin = table.Column<DateTime>(nullable: false),
                    UserStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });


            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NameMultiLangJson = table.Column<string>(nullable: true),
                    CountryID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Alias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Regions_Countries_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Countries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyRates",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CurrencyID = table.Column<long>(nullable: false),
                    CurrencyBaseRateID = table.Column<long>(nullable: false),
                    FaceValue = table.Column<int>(nullable: false),
                    Rate = table.Column<float>(nullable: false),
                    CurrencyBaseRateID1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CurrencyRates_Currencies_CurrencyBaseRateID",
                        column: x => x.CurrencyBaseRateID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrencyRates_Currencies_CurrencyBaseRateID1",
                        column: x => x.CurrencyBaseRateID1,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NameMultiLangJson = table.Column<string>(nullable: true),
                    PropertyGroupID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UnitMeasure = table.Column<string>(maxLength: 6, nullable: true),
                    ControlType = table.Column<int>(nullable: false),
                    ActiveStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Properties_PropertyGroups_PropertyGroupID",
                        column: x => x.PropertyGroupID,
                        principalTable: "PropertyGroups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RoleId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NameMultiLangJson = table.Column<string>(nullable: true),
                    SuperCategID = table.Column<long>(nullable: false),
                    CategGroupID = table.Column<long>(nullable: false),
                    Alias = table.Column<string>(nullable: true),
                    HideModelField = table.Column<bool>(nullable: false),
                    NameMultiLangSingularJson = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 120, nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Categories_CategGroups_CategGroupID",
                        column: x => x.CategGroupID,
                        principalTable: "CategGroups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Categories_SuperCategories_SuperCategID",
                        column: x => x.SuperCategID,
                        principalTable: "SuperCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Balances",
                columns: table => new
                {
                    UserID = table.Column<long>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    LastAddDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balances", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Balances_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPhones",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserID = table.Column<long>(nullable: false),
                    Number = table.Column<string>(maxLength: 16, nullable: true),
                    PriorityStatus = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPhones", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserPhones_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NameMultiLangJson = table.Column<string>(nullable: true),
                    RegionID = table.Column<long>(nullable: false),
                    CountryID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Alias = table.Column<string>(nullable: true),
                    Weight = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cities_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyCheckBoxGroups",
                columns: table => new
                {
                    PropertyID = table.Column<long>(nullable: false),
                    CheckBoxGroupID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyCheckBoxGroups", x => new { x.PropertyID, x.CheckBoxGroupID });
                    table.ForeignKey(
                        name: "FK_PropertyCheckBoxGroups_CheckBoxGroups_CheckBoxGroupID",
                        column: x => x.CheckBoxGroupID,
                        principalTable: "CheckBoxGroups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyCheckBoxGroups_Properties_PropertyID",
                        column: x => x.PropertyID,
                        principalTable: "Properties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectOptions",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NameMultiLangJson = table.Column<string>(nullable: true),
                    PropertyID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    MetaDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectOptions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SelectOptions_Properties_PropertyID",
                        column: x => x.PropertyID,
                        principalTable: "Properties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategBrands",
                columns: table => new
                {
                    CategID = table.Column<long>(nullable: false),
                    BrandID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategBrands", x => new { x.CategID, x.BrandID });
                    table.ForeignKey(
                        name: "FK_CategBrands_Brands_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brands",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategBrands_Categories_CategID",
                        column: x => x.CategID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategTypes",
                columns: table => new
                {
                    CategID = table.Column<long>(nullable: false),
                    ItemTypeID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategTypes", x => new { x.CategID, x.ItemTypeID });
                    table.ForeignKey(
                        name: "FK_CategTypes_Categories_CategID",
                        column: x => x.CategID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategTypes_Type_ItemTypeID",
                        column: x => x.ItemTypeID,
                        principalTable: "Type",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Model",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CategID = table.Column<long>(nullable: false),
                    ItemTypeID = table.Column<long>(nullable: false),
                    BrandID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    WatchWeightStatus = table.Column<int>(nullable: false),
                    LikeValue = table.Column<long>(nullable: false),
                    LikeCount = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Model", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Model_Brands_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brands",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Model_Categories_CategID",
                        column: x => x.CategID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Model_Type_ItemTypeID",
                        column: x => x.ItemTypeID,
                        principalTable: "Type",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyCategs",
                columns: table => new
                {
                    PropertyID = table.Column<long>(nullable: false),
                    CategID = table.Column<long>(nullable: false),
                    AddToFilter = table.Column<int>(nullable: false),
                    AddToCard = table.Column<int>(nullable: false),
                    Priority = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyCategs", x => new { x.PropertyID, x.CategID });
                    table.ForeignKey(
                        name: "FK_PropertyCategs_Categories_CategID",
                        column: x => x.CategID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyCategs_Properties_PropertyID",
                        column: x => x.PropertyID,
                        principalTable: "Properties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BalanceTransactions",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BalanceUserID = table.Column<long>(nullable: false),
                    PaymentSourceID = table.Column<long>(nullable: false),
                    CurrencyID = table.Column<long>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    DateTransaction = table.Column<DateTime>(nullable: false),
                    BalanceTransactionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceTransactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BalanceTransactions_Balances_BalanceUserID",
                        column: x => x.BalanceUserID,
                        principalTable: "Balances",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BalanceTransactions_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dealerships",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CityId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    SubwayStations = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    MapCoords = table.Column<string>(nullable: true),
                    OfficialDealer = table.Column<bool>(nullable: false),
                    MoWorkTime = table.Column<string>(maxLength: 15, nullable: true),
                    TuWorkTime = table.Column<string>(maxLength: 15, nullable: true),
                    WeWorkTime = table.Column<string>(maxLength: 15, nullable: true),
                    ThWorkTime = table.Column<string>(maxLength: 15, nullable: true),
                    FrWorkTime = table.Column<string>(maxLength: 15, nullable: true),
                    SaWorkTime = table.Column<string>(maxLength: 15, nullable: true),
                    SuWorkTime = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dealerships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dealerships_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dealerships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdVehicles",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserID = table.Column<long>(nullable: false),
                    SuperCategID = table.Column<long>(nullable: false),
                    BrandID = table.Column<long>(nullable: false),
                    TypeID = table.Column<long>(nullable: false),
                    CityID = table.Column<long>(nullable: false),
                    RegionID = table.Column<long>(nullable: true),
                    CountryID = table.Column<long>(nullable: false),
                    CurrencyID = table.Column<long>(nullable: false),
                    Year = table.Column<string>(maxLength: 4, nullable: true),
                    Price = table.Column<double>(nullable: false),
                    MinimalPrice = table.Column<double>(nullable: true),
                    NewModelName = table.Column<string>(maxLength: 100, nullable: true),
                    InstalmentSelling = table.Column<bool>(nullable: false),
                    CustomsCleared = table.Column<bool>(nullable: false),
                    HotSelling = table.Column<bool>(nullable: false),
                    ExchangePossible = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(maxLength: 2048, nullable: true),
                    Title = table.Column<string>(maxLength: 1024, nullable: true),
                    CategID = table.Column<long>(nullable: false),
                    ModelID = table.Column<long>(nullable: true),
                    DealershipId = table.Column<long>(nullable: true),
                    ImgJson = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 2048, nullable: true),
                    FolderImgName = table.Column<string>(maxLength: 32, nullable: true),
                    MapJsonCoord = table.Column<string>(nullable: true),
                    ViewCount = table.Column<long>(nullable: false),
                    ImgExists = table.Column<bool>(nullable: false),
                    AddDate = table.Column<DateTime>(nullable: false),
                    DateExpired = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    AdStatus = table.Column<int>(nullable: false),
                    IsAutoUpdatable = table.Column<int>(nullable: false),
                    ModirationStatus = table.Column<int>(nullable: false),
                    CommentRestriction = table.Column<int>(nullable: false),
                    ModelVerification = table.Column<int>(nullable: false),
                    Condition = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdVehicles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AdVehicles_Brands_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brands",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdVehicles_Categories_CategID",
                        column: x => x.CategID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdVehicles_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdVehicles_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdVehicles_Dealerships_DealershipId",
                        column: x => x.DealershipId,
                        principalTable: "Dealerships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdVehicles_Model_ModelID",
                        column: x => x.ModelID,
                        principalTable: "Model",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdVehicles_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdVehicles_SuperCategories_SuperCategID",
                        column: x => x.SuperCategID,
                        principalTable: "SuperCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdVehicles_Type_TypeID",
                        column: x => x.TypeID,
                        principalTable: "Type",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdVehicles_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdVehicleServiceStores",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NameMultiLangJson = table.Column<string>(nullable: true),
                    AdVehicleID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MetaData = table.Column<string>(nullable: true),
                    ActiveStatus = table.Column<int>(nullable: false),
                    BoughtDate = table.Column<DateTime>(nullable: true),
                    LastDate = table.Column<DateTime>(nullable: true),
                    ServiceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdVehicleServiceStores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AdVehicleServiceStores_AdVehicles_AdVehicleID",
                        column: x => x.AdVehicleID,
                        principalTable: "AdVehicles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DynamicPropertyAdVehicles",
                columns: table => new
                {
                    AdVehicleID = table.Column<long>(nullable: false),
                    PropertyID = table.Column<long>(nullable: false),
                    PropertyValue = table.Column<string>(maxLength: 14, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicPropertyAdVehicles", x => new { x.AdVehicleID, x.PropertyID });
                    table.ForeignKey(
                        name: "FK_DynamicPropertyAdVehicles_AdVehicles_AdVehicleID",
                        column: x => x.AdVehicleID,
                        principalTable: "AdVehicles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DynamicPropertyAdVehicles_Properties_PropertyID",
                        column: x => x.PropertyID,
                        principalTable: "Properties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdVehicles_BrandID",
                table: "AdVehicles",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_AdVehicles_CategID",
                table: "AdVehicles",
                column: "CategID");

            migrationBuilder.CreateIndex(
                name: "IX_AdVehicles_CityID",
                table: "AdVehicles",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_AdVehicles_CurrencyID",
                table: "AdVehicles",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_AdVehicles_DealershipId",
                table: "AdVehicles",
                column: "DealershipId");

            migrationBuilder.CreateIndex(
                name: "IX_AdVehicles_ModelID",
                table: "AdVehicles",
                column: "ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_AdVehicles_RegionID",
                table: "AdVehicles",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_AdVehicles_SuperCategID",
                table: "AdVehicles",
                column: "SuperCategID");

            migrationBuilder.CreateIndex(
                name: "IX_AdVehicles_TypeID",
                table: "AdVehicles",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_AdVehicles_UserID",
                table: "AdVehicles",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_AdVehicleServiceStores_AdVehicleID",
                table: "AdVehicleServiceStores",
                column: "AdVehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceTransactions_BalanceUserID",
                table: "BalanceTransactions",
                column: "BalanceUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceTransactions_CurrencyID",
                table: "BalanceTransactions",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Name",
                table: "Brands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategBrands_BrandID",
                table: "CategBrands",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategGroupID",
                table: "Categories",
                column: "CategGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SuperCategID",
                table: "Categories",
                column: "SuperCategID");

            migrationBuilder.CreateIndex(
                name: "IX_CategTypes_ItemTypeID",
                table: "CategTypes",
                column: "ItemTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckBoxGroups_Name",
                table: "CheckBoxGroups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_RegionID",
                table: "Cities",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentParentID",
                table: "Comments",
                column: "CommentParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Name",
                table: "Currencies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRates_CurrencyBaseRateID",
                table: "CurrencyRates",
                column: "CurrencyBaseRateID");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRates_CurrencyBaseRateID1",
                table: "CurrencyRates",
                column: "CurrencyBaseRateID1");

            migrationBuilder.CreateIndex(
                name: "IX_Dealerships_CityId",
                table: "Dealerships",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealerships_UserId",
                table: "Dealerships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicPropertyAdVehicles_PropertyID",
                table: "DynamicPropertyAdVehicles",
                column: "PropertyID");

            migrationBuilder.CreateIndex(
                name: "IX_Model_BrandID",
                table: "Model",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Model_CategID",
                table: "Model",
                column: "CategID");

            migrationBuilder.CreateIndex(
                name: "IX_Model_ItemTypeID",
                table: "Model",
                column: "ItemTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Name",
                table: "Properties",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyGroupID",
                table: "Properties",
                column: "PropertyGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyCategs_CategID",
                table: "PropertyCategs",
                column: "CategID");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyCheckBoxGroups_CheckBoxGroupID",
                table: "PropertyCheckBoxGroups",
                column: "CheckBoxGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_CountryID",
                table: "Regions",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SelectOptions_PropertyID",
                table: "SelectOptions",
                column: "PropertyID");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePrices_SevicePriceName",
                table: "ServicePrices",
                column: "SevicePriceName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuperCategories_Name",
                table: "SuperCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Type_Name",
                table: "Type",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPhones_UserID",
                table: "UserPhones",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);


            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "AvatarImgName", "ConcurrencyStamp", "Email", "EmailConfirmed", "LastLogin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RegistrationDate", "SecurityStamp", "TwoFactorEnabled", "UserLastName", "UserName", "UserNickName", "UserStatus" },
                values: new object[] { 1L, 0, null, "9ac88e11-e377-4a49-bef9-ed7767ff6f9d", "admin@mail.ru", false, new DateTime(2019, 3, 26, 19, 7, 9, 251, DateTimeKind.Local).AddTicks(6890), false, null, "ADMIN@MAIL.RU", "ADMIN@MAIL.RU", "AQAAAAEAACcQAAAAEA7vpzNBUIMLvB4bdfb8xX5IIsMZ86GfG1In4YX3q8BYyZoFQYSGuVOVWB3XfCdZOA==", null, false, new DateTime(2019, 3, 26, 19, 7, 9, 250, DateTimeKind.Local).AddTicks(3818), "6FPVSOIY6BCPOQH4BZUCNYIKQ5WB4VRM", false, null, "admin@mail.ru", "admin@mail.ru", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DropTable(
                name: "AdVehicleServiceStores");

            migrationBuilder.DropTable(
                name: "BalanceTransactions");

            migrationBuilder.DropTable(
                name: "CategBrands");

            migrationBuilder.DropTable(
                name: "CategTypes");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CurrencyRates");

            migrationBuilder.DropTable(
                name: "DynamicPropertyAdVehicles");

            migrationBuilder.DropTable(
                name: "FeedBacks");

            migrationBuilder.DropTable(
                name: "PropertyCategs");

            migrationBuilder.DropTable(
                name: "PropertyCheckBoxGroups");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SelectOptions");

            migrationBuilder.DropTable(
                name: "ServicePrices");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserPhones");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Balances");

            migrationBuilder.DropTable(
                name: "AdVehicles");

            migrationBuilder.DropTable(
                name: "CheckBoxGroups");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Dealerships");

            migrationBuilder.DropTable(
                name: "Model");

            migrationBuilder.DropTable(
                name: "PropertyGroups");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "CategGroups");

            migrationBuilder.DropTable(
                name: "SuperCategories");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
