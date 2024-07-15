using Microsoft.EntityFrameworkCore;
using SmartChef;
using SmartChef.Models;
using Microsoft.Extensions.Logging;

namespace SmartChef.API
{
    public static class UsersAPI
    {
        public static void Map(WebApplication app)
        {
            //Check User
            app.MapGet("/checkUser/{uid}", (SmartChefDbContext db, string uid) =>
            {
                var user = db.Users.FirstOrDefault(u => u.Uid == uid);

                if (user == null)
                {
                    return Results.NotFound("User not registered");
                }

                return Results.Ok(user);
            });

            //Register User
            app.MapPost("/users/register", (SmartChefDbContext db, User newUser) =>
            {
                try
                {
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    return Results.Created($"/users/{newUser.Id}", newUser);
                }
                catch (DbUpdateException)
                {
                    return Results.BadRequest("Unable to register user");
                }
            });

            // Get user by ID
            app.MapGet("/users/{userId}", async (SmartChefDbContext db, int userId, ILogger<Program> logger) =>
            {
                try
                {
                    var user = await db.Users.FindAsync(userId);

                    if (user == null)
                    {
                        logger.LogWarning($"User with ID {userId} not found.");
                        return Results.NotFound(new { message = "User not found." });
                    }

                    return Results.Ok(user);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error fetching user with ID {userId}: {ex.Message}");
                    return Results.Problem("Internal server error");
                }
            });

            // Get all users
            app.MapGet("/users", async (SmartChefDbContext db, ILogger<Program> logger) =>
            {
                try
                {
                    var users = await db.Users.ToListAsync();
                    return Results.Ok(users);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error fetching all users: {ex.Message}");
                    return Results.Problem("Internal server error");
                }
            });

            // Update user
            app.MapPut("/users/{userId}", async (SmartChefDbContext db, int userId, User updatedUser, ILogger<Program> logger) =>
            {
                try
                {
                    var user = await db.Users.FindAsync(userId);

                    if (user == null)
                    {
                        logger.LogWarning($"User with ID {userId} not found.");
                        return Results.NotFound(new { message = "User not found." });
                    }

                    // Update the user's details
                    user.UserName = updatedUser.UserName ?? user.UserName;
                    user.Email = updatedUser.Email ?? user.Email;

                    await db.SaveChangesAsync();

                    return Results.Ok(user);
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"Error updating user with ID {userId}: {ex.Message}");
                    return Results.BadRequest(new { message = "Unable to update user" });
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error updating user with ID {userId}: {ex.Message}");
                    return Results.Problem("Internal server error");
                }
            });

            // Delete user
            app.MapDelete("/users/{userId}", async (SmartChefDbContext db, int userId, ILogger<Program> logger) =>
            {
                try
                {
                    var user = await db.Users.FindAsync(userId);

                    if (user == null)
                    {
                        logger.LogWarning($"User with ID {userId} not found.");
                        return Results.NotFound(new { message = "User not found." });
                    }

                    db.Users.Remove(user);
                    await db.SaveChangesAsync();

                    return Results.Ok(new { message = "User successfully deleted." });
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error deleting user with ID {userId}: {ex.Message}");
                    return Results.Problem("Internal server error");
                }
            });
        }
    }
}





