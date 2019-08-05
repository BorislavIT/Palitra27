using Microsoft.EntityFrameworkCore.Migrations;

namespace Palitra27.Data.Migrations
{
    public partial class jkga : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProduct_FavouriteLists_FavouriteListId",
                table: "FavouriteProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProduct_Products_ProductId",
                table: "FavouriteProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavouriteProduct",
                table: "FavouriteProduct");

            migrationBuilder.RenameTable(
                name: "FavouriteProduct",
                newName: "FavouriteProducts");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteProduct_FavouriteListId",
                table: "FavouriteProducts",
                newName: "IX_FavouriteProducts_FavouriteListId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavouriteProducts",
                table: "FavouriteProducts",
                columns: new[] { "ProductId", "FavouriteListId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProducts_FavouriteLists_FavouriteListId",
                table: "FavouriteProducts",
                column: "FavouriteListId",
                principalTable: "FavouriteLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProducts_Products_ProductId",
                table: "FavouriteProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProducts_FavouriteLists_FavouriteListId",
                table: "FavouriteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProducts_Products_ProductId",
                table: "FavouriteProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavouriteProducts",
                table: "FavouriteProducts");

            migrationBuilder.RenameTable(
                name: "FavouriteProducts",
                newName: "FavouriteProduct");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteProducts_FavouriteListId",
                table: "FavouriteProduct",
                newName: "IX_FavouriteProduct_FavouriteListId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavouriteProduct",
                table: "FavouriteProduct",
                columns: new[] { "ProductId", "FavouriteListId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProduct_FavouriteLists_FavouriteListId",
                table: "FavouriteProduct",
                column: "FavouriteListId",
                principalTable: "FavouriteLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProduct_Products_ProductId",
                table: "FavouriteProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
