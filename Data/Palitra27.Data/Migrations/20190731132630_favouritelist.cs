using Microsoft.EntityFrameworkCore.Migrations;

namespace Palitra27.Data.Migrations
{
    public partial class FavouriteList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavouriteListId",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FavouriteListId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FavouriteLists",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavouriteLists_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_FavouriteLists_FavouriteListId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "FavouriteLists");

            migrationBuilder.DropIndex(
                name: "IX_Products_FavouriteListId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FavouriteListId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FavouriteListId",
                table: "AspNetUsers");
        }
    }
}
