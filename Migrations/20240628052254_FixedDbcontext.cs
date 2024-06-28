using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartChef.Migrations
{
    /// <inheritdoc />
    public partial class FixedDbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1,
                column: "Ingredients",
                value: "Spaghetti, Ground Beef, Tomato Sauce");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 2,
                column: "Ingredients",
                value: "Bread, Cheese, Butter");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1,
                column: "Ingredients",
                value: "[\"Spaghetti\", \"Ground Beef\", \"Tomato Sauce\"]");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 2,
                column: "Ingredients",
                value: "[\"Bread\", \"Cheese\", \"Butter\"]");
        }
    }
}
