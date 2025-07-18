﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "DeletedRecords",
                columns: table => new
                {
                    DeletedRecordID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordDeleteDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TableSourceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableSourceDeletedRecordID = table.Column<long>(type: "bigint", nullable: false),
                    TableSourceDeletedRecordValueFromJSON = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedRecords", x => x.DeletedRecordID);
                });

            migrationBuilder.CreateTable(
                name: "OptionalParameters",
                columns: table => new
                {
                    OptionalParameterID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionalParameterName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionalParameters", x => x.OptionalParameterID);
                });

            migrationBuilder.CreateTable(
                name: "Pricelists",
                columns: table => new
                {
                    PricelistID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PricelistName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PriceListDateCreation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PriceListDateModification = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pricelists", x => x.PricelistID);
                });

            migrationBuilder.CreateTable(
                name: "ProductQuantityTypes",
                columns: table => new
                {
                    ProductQuantityTypeID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductQuantityTypeName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductQuantityTypes", x => x.ProductQuantityTypeID);
                });

            migrationBuilder.CreateTable(
                name: "ProductQuantities",
                columns: table => new
                {
                    ProductQuantityID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductQuantityName = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ProductQuantityTypeID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductQuantities", x => x.ProductQuantityID);
                    table.ForeignKey(
                        name: "FK_ProductQuantities_ProductQuantityTypes_ProductQuantityTypeID",
                        column: x => x.ProductQuantityTypeID,
                        principalTable: "ProductQuantityTypes",
                        principalColumn: "ProductQuantityTypeID");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CategoryID = table.Column<long>(type: "bigint", nullable: false),
                    ProductPrice = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    ProductQuantityID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductQuantities_ProductQuantityID",
                        column: x => x.ProductQuantityID,
                        principalTable: "ProductQuantities",
                        principalColumn: "ProductQuantityID");
                });

            migrationBuilder.CreateTable(
                name: "PricelistProductPurchases",
                columns: table => new
                {
                    PurchaseID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PricelistID = table.Column<long>(type: "bigint", nullable: true),
                    ProductID = table.Column<long>(type: "bigint", nullable: true),
                    ProductQuantityNumber = table.Column<double>(type: "float", nullable: false),
                    ProductPriceAtBuy = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    ProductNameAtBuy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductQuantityNameAtBuy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricelistProductPurchases", x => x.PurchaseID);
                    table.ForeignKey(
                        name: "FK_PricelistProductPurchases_Pricelists_PricelistID",
                        column: x => x.PricelistID,
                        principalTable: "Pricelists",
                        principalColumn: "PricelistID");
                    table.ForeignKey(
                        name: "FK_PricelistProductPurchases_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "PricelistOptionalParameters",
                columns: table => new
                {
                    OptionalParameterEntryID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionalParameterID = table.Column<long>(type: "bigint", nullable: true),
                    OptionalParameterValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PricelistPurchaseID = table.Column<long>(type: "bigint", nullable: true),
                    PricelistProductPurchasePurchaseID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricelistOptionalParameters", x => x.OptionalParameterEntryID);
                    table.ForeignKey(
                        name: "FK_PricelistOptionalParameters_OptionalParameters_OptionalParameterID",
                        column: x => x.OptionalParameterID,
                        principalTable: "OptionalParameters",
                        principalColumn: "OptionalParameterID");
                    table.ForeignKey(
                        name: "FK_PricelistOptionalParameters_PricelistProductPurchases_PricelistProductPurchasePurchaseID",
                        column: x => x.PricelistProductPurchasePurchaseID,
                        principalTable: "PricelistProductPurchases",
                        principalColumn: "PurchaseID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PricelistOptionalParameters_OptionalParameterID",
                table: "PricelistOptionalParameters",
                column: "OptionalParameterID");

            migrationBuilder.CreateIndex(
                name: "IX_PricelistOptionalParameters_PricelistProductPurchasePurchaseID",
                table: "PricelistOptionalParameters",
                column: "PricelistProductPurchasePurchaseID");

            migrationBuilder.CreateIndex(
                name: "IX_PricelistProductPurchases_PricelistID",
                table: "PricelistProductPurchases",
                column: "PricelistID");

            migrationBuilder.CreateIndex(
                name: "IX_PricelistProductPurchases_ProductID",
                table: "PricelistProductPurchases",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductQuantities_ProductQuantityTypeID",
                table: "ProductQuantities",
                column: "ProductQuantityTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductQuantityID",
                table: "Products",
                column: "ProductQuantityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedRecords");

            migrationBuilder.DropTable(
                name: "PricelistOptionalParameters");

            migrationBuilder.DropTable(
                name: "OptionalParameters");

            migrationBuilder.DropTable(
                name: "PricelistProductPurchases");

            migrationBuilder.DropTable(
                name: "Pricelists");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ProductQuantities");

            migrationBuilder.DropTable(
                name: "ProductQuantityTypes");
        }
    }
}
