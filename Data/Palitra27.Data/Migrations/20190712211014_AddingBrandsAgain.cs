using Microsoft.EntityFrameworkCore.Migrations;

namespace Palitra27.Data.Migrations
{
    public partial class AddingBrandsAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductBrand_BrandId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductBrand",
                table: "ProductBrand");

            migrationBuilder.RenameTable(
                name: "ProductBrand",
                newName: "ProductsBrands");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsBrands",
                table: "ProductsBrands",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductsBrands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "ProductsBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductsBrands_BrandId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsBrands",
                table: "ProductsBrands");

            migrationBuilder.RenameTable(
                name: "ProductsBrands",
                newName: "ProductBrand");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductBrand",
                table: "ProductBrand",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductBrand_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "ProductBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
