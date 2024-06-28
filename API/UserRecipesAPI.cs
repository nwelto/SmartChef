using Microsoft.EntityFrameworkCore;
using SmartChef;
using SmartChef.Models;

namespace SmartChef.API
{
    public static class UserRecipesAPI
    {
        public static void Map(WebApplication app)
        {
            // Add a new UserRecipe
            app.MapPost("/userrecipes", async (SmartChefDbContext db, UserRecipe newUserRecipe) =>
            {
                // Check if the user and recipe exist
                var user = await db.Users.FindAsync(newUserRecipe.UserId);
                var recipe = await db.Recipes.FindAsync(newUserRecipe.RecipeId);

                if (user == null || recipe == null)
                {
                    return Results.BadRequest("Invalid user or recipe.");
                }

                db.UserRecipes.Add(newUserRecipe);
                await db.SaveChangesAsync();
                return Results.Created($"/userrecipes/{newUserRecipe.UserRecipeId}", newUserRecipe);
            });

            // Get all recipes saved by a specific user
            app.MapGet("/userrecipes/user/{userId}", async (SmartChefDbContext db, int userId) =>
            {
                var userRecipes = await db.UserRecipes
                    .Where(ur => ur.UserId == userId)
                    .Include(ur => ur.Recipe)
                    .Select(ur => new
                    {
                        ur.UserRecipeId,
                        ur.UserId,
                        ur.RecipeId,
                        ur.Recipe.Title,
                        ur.Recipe.Instructions,
                        ur.Recipe.Ingredients,
                        ur.Recipe.ImageUrl,
                        ur.Recipe.SourceUrl
                    })
                    .ToListAsync();

                if (!userRecipes.Any())
                {
                    return Results.NotFound("No recipes found for this user.");
                }

                return Results.Ok(userRecipes);
            });

            // Get a specific UserRecipe by ID
            app.MapGet("/userrecipes/{userRecipeId}", async (SmartChefDbContext db, int userRecipeId) =>
            {
                var userRecipe = await db.UserRecipes
                    .Include(ur => ur.Recipe)
                    .Where(ur => ur.UserRecipeId == userRecipeId)
                    .Select(ur => new
                    {
                        ur.UserRecipeId,
                        ur.UserId,
                        ur.RecipeId,
                        ur.Recipe.Title,
                        ur.Recipe.Instructions,
                        ur.Recipe.Ingredients,
                        ur.Recipe.ImageUrl,
                        ur.Recipe.SourceUrl
                    })
                    .FirstOrDefaultAsync();

                if (userRecipe == null)
                {
                    return Results.NotFound("UserRecipe not found.");
                }

                return Results.Ok(userRecipe);
            });

            // Delete a UserRecipe by ID
            app.MapDelete("/userrecipes/{userRecipeId}", async (SmartChefDbContext db, int userRecipeId) =>
            {
                var userRecipe = await db.UserRecipes.FindAsync(userRecipeId);

                if (userRecipe == null)
                {
                    return Results.NotFound("UserRecipe not found.");
                }

                db.UserRecipes.Remove(userRecipe);
                await db.SaveChangesAsync();

                return Results.Ok("UserRecipe successfully deleted.");
            });
        }
    }
}

