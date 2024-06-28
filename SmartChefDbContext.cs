using Microsoft.EntityFrameworkCore;
using SmartChef.Models;

public class SmartChefDbContext : DbContext
{
    public SmartChefDbContext(DbContextOptions<SmartChefDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<UserRecipe> UserRecipes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<User>().HasData(
            new User { UserId = 1, FirebaseUid = "firebase-uid-1", Email = "user1@example.com", Username = "user1" },
            new User { UserId = 2, FirebaseUid = "firebase-uid-2", Email = "user2@example.com", Username = "user2" }
        );

        modelBuilder.Entity<Recipe>().HasData(
            new Recipe
            {
                RecipeId = 1,
                Title = "Spaghetti Bolognese",
                Instructions = "Cook spaghetti. Prepare the sauce...",
                Ingredients = "Spaghetti, Ground Beef, Tomato Sauce",
                ImageUrl = "http://example.com/spaghetti.jpg",
                SourceUrl = "http://example.com/spaghetti-recipe"
            },
            new Recipe
            {
                RecipeId = 2,
                Title = "Grilled Cheese Sandwich",
                Instructions = "Butter the bread. Grill the cheese...",
                Ingredients = "Bread, Cheese, Butter",
                ImageUrl = "http://example.com/grilledcheese.jpg",
                SourceUrl = "http://example.com/grilledcheese-recipe"
            }
        );

        modelBuilder.Entity<UserRecipe>().HasData(
            new UserRecipe { UserRecipeId = 1, UserId = 1, RecipeId = 1 },
            new UserRecipe { UserRecipeId = 2, UserId = 2, RecipeId = 2 }
        );

        // Configure relationships
        modelBuilder.Entity<UserRecipe>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRecipes)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRecipe>()
            .HasOne(ur => ur.Recipe)
            .WithMany(r => r.UserRecipes)
            .HasForeignKey(ur => ur.RecipeId);
    }
}





