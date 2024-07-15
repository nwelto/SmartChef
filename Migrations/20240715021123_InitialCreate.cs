using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartChef.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Instructions = table.Column<string>(type: "text", nullable: false),
                    Ingredients = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    SourceUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRecipes",
                columns: table => new
                {
                    UserRecipeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RecipeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecipes", x => x.UserRecipeId);
                    table.ForeignKey(
                        name: "FK_UserRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRecipes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "ImageUrl", "Ingredients", "Instructions", "SourceUrl", "Title" },
                values: new object[,]
                {
                    { 1, "http://example.com/spaghetti.jpg", "Spaghetti, Ground Beef, Tomato Sauce", "Cook spaghetti. Prepare the sauce...", "http://example.com/spaghetti-recipe", "Spaghetti Bolognese" },
                    { 2, "http://example.com/grilledcheese.jpg", "Bread, Cheese, Butter", "Butter the bread. Grill the cheese...", "http://example.com/grilledcheese-recipe", "Grilled Cheese Sandwich" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Uid", "UserName" },
                values: new object[,]
                {
                    { 1, "user1@example.com", "firebase-uid-1", "user1" },
                    { 2, "user2@example.com", "firebase-uid-2", "user2" }
                });

            migrationBuilder.InsertData(
                table: "UserRecipes",
                columns: new[] { "UserRecipeId", "RecipeId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipes_RecipeId",
                table: "UserRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipes_UserId",
                table: "UserRecipes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRecipes");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
