using Microsoft.EntityFrameworkCore.Migrations;

namespace Arenda.Migrations
{
    public partial class AddCityIdToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "city_id",
                table: "users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_users_city_id",
                table: "users",
                column: "city_id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_cities_city_id",
                table: "users",
                column: "city_id",
                principalTable: "cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_cities_city_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_city_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "city_id",
                table: "users");
        }
    }
}