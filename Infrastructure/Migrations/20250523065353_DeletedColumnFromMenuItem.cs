using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeletedColumnFromMenuItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItem_FoodType_FoodTypeId",
                table: "MenuItem");

            migrationBuilder.DropIndex(
                name: "IX_MenuItem_FoodTypeId",
                table: "MenuItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_FoodTypeId",
                table: "MenuItem",
                column: "FoodTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItem_FoodType_FoodTypeId",
                table: "MenuItem",
                column: "FoodTypeId",
                principalTable: "FoodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
