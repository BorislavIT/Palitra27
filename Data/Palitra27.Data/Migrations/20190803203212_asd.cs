using Microsoft.EntityFrameworkCore.Migrations;

namespace Palitra27.Data.Migrations
{
    public partial class asd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_FavouriteLists_FavouriteListId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FavouriteListId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FavouriteListId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "FavouriteProduct",
                columns: table => new
                {
                    FavouriteListId = table.Column<string>(nullable: false),
                    ProductId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteProduct", x => new { x.ProductId, x.FavouriteListId });
                    table.ForeignKey(
                        name: "FK_FavouriteProduct_FavouriteLists_FavouriteListId",
                        column: x => x.FavouriteListId,
                        principalTable: "FavouriteLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavouriteProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteProduct_FavouriteListId",
                table: "FavouriteProduct",
                column: "FavouriteListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavouriteProduct");

            migrationBuilder.AddColumn<string>(
                name: "FavouriteListId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FavouriteListId",
                table: "Products",
                column: "FavouriteListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_FavouriteLists_FavouriteListId",
                table: "Products",
                column: "FavouriteListId",
                principalTable: "FavouriteLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
