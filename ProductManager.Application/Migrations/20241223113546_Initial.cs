using System;
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
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "OptionalParameters",
                columns: table => new
                {
                    OptionalParameterID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionalParameterType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionalParameterName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionalParameters", x => x.OptionalParameterID);
                });

            migrationBuilder.CreateTable(
                name: "PriceLists",
                columns: table => new
                {
                    PriceListID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceListName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceListDateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PriceListDateModyfycation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLists", x => x.PriceListID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryID = table.Column<long>(type: "bigint", nullable: false),
                    ProductPrice = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "PriceListOptionalParameters",
                columns: table => new
                {
                    OptionalParameterEntryID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionalParameterID = table.Column<long>(type: "bigint", nullable: false),
                    OptionalParameterValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceListLineID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceListOptionalParameters", x => x.OptionalParameterEntryID);
                    table.ForeignKey(
                        name: "FK_PriceListOptionalParameters_OptionalParameters_OptionalParameterID",
                        column: x => x.OptionalParameterID,
                        principalTable: "OptionalParameters",
                        principalColumn: "OptionalParameterID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceListProducts",
                columns: table => new
                {
                    PriceListLineID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceListOptionalParametersOptionalParameterEntryID = table.Column<long>(type: "bigint", nullable: false),
                    PriceListID = table.Column<long>(type: "bigint", nullable: false),
                    ProductID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceListProducts", x => x.PriceListLineID);
                    table.ForeignKey(
                        name: "FK_PriceListProducts_PriceListOptionalParameters_PriceListOptionalParametersOptionalParameterEntryID",
                        column: x => x.PriceListOptionalParametersOptionalParameterEntryID,
                        principalTable: "PriceListOptionalParameters",
                        principalColumn: "OptionalParameterEntryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceListPriceListProduct",
                columns: table => new
                {
                    PriceListProductsPriceListLineID = table.Column<long>(type: "bigint", nullable: false),
                    PriceListsPriceListID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceListPriceListProduct", x => new { x.PriceListProductsPriceListLineID, x.PriceListsPriceListID });
                    table.ForeignKey(
                        name: "FK_PriceListPriceListProduct_PriceListProducts_PriceListProductsPriceListLineID",
                        column: x => x.PriceListProductsPriceListLineID,
                        principalTable: "PriceListProducts",
                        principalColumn: "PriceListLineID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceListPriceListProduct_PriceLists_PriceListsPriceListID",
                        column: x => x.PriceListsPriceListID,
                        principalTable: "PriceLists",
                        principalColumn: "PriceListID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceListProductProduct",
                columns: table => new
                {
                    PriceListProductsPriceListLineID = table.Column<long>(type: "bigint", nullable: false),
                    ProductsProductID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceListProductProduct", x => new { x.PriceListProductsPriceListLineID, x.ProductsProductID });
                    table.ForeignKey(
                        name: "FK_PriceListProductProduct_PriceListProducts_PriceListProductsPriceListLineID",
                        column: x => x.PriceListProductsPriceListLineID,
                        principalTable: "PriceListProducts",
                        principalColumn: "PriceListLineID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceListProductProduct_Products_ProductsProductID",
                        column: x => x.ProductsProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceListOptionalParameters_OptionalParameterID",
                table: "PriceListOptionalParameters",
                column: "OptionalParameterID");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListPriceListProduct_PriceListsPriceListID",
                table: "PriceListPriceListProduct",
                column: "PriceListsPriceListID");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListProductProduct_ProductsProductID",
                table: "PriceListProductProduct",
                column: "ProductsProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListProducts_PriceListOptionalParametersOptionalParameterEntryID",
                table: "PriceListProducts",
                column: "PriceListOptionalParametersOptionalParameterEntryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceListPriceListProduct");

            migrationBuilder.DropTable(
                name: "PriceListProductProduct");

            migrationBuilder.DropTable(
                name: "PriceLists");

            migrationBuilder.DropTable(
                name: "PriceListProducts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "PriceListOptionalParameters");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "OptionalParameters");
        }
    }
}
