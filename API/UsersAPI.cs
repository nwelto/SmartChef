using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using SmartChef;
using SmartChef.Models;
using Microsoft.AspNetCore.Http;

namespace SmartChef.API
{
    public static class UsersAPI
    {
        public static void Map(WebApplication app)
        {
            // Check User
            app.MapPost("/checkUser", async (SmartChefDbContext db, HttpContext context) =>
            {
                if (!context.Items.ContainsKey("FirebaseUid"))
                {
                    return Results.Unauthorized();
                }

                string uid = context.Items["FirebaseUid"].ToString();
                var user = await db.Users.Where(u => u.FirebaseUid == uid).FirstOrDefaultAsync();

                if (user == null)
                {
                    // Assuming you have user details from the token or request, create a new user
                    var userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
                    user = new User
                    {
                        FirebaseUid = uid,
                        Email = userRecord.Email,
                        Username = userRecord.DisplayName
                    };

                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                }

                var userInfo = new
                {
                    user.UserId,
                    user.Username,
                    user.Email,
                    user.FirebaseUid
                };

                return Results.Ok(userInfo);
            });

            // Add a new user
            app.MapPost("/user", async (SmartChefDbContext db, User newUser) =>
            {
                if (string.IsNullOrEmpty(newUser.FirebaseUid) || string.IsNullOrEmpty(newUser.Email) || string.IsNullOrEmpty(newUser.Username))
                {
                    return Results.BadRequest("Invalid user data.");
                }

                db.Users.Add(newUser);
                await db.SaveChangesAsync();
                return Results.Created($"/user/{newUser.UserId}", newUser);
            });

            // Get user by ID
            app.MapGet("/users/{userId}", async (SmartChefDbContext db, int userId) =>
            {
                var user = await db.Users.FindAsync(userId);

                if (user == null)
                {
                    return Results.NotFound("User not found.");
                }

                var userDetails = new
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email
                };

                return Results.Ok(userDetails);
            });

            // Get all users
            app.MapGet("/users", async (SmartChefDbContext db) =>
            {
                var users = await db.Users.Select(user => new
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email
                }).ToListAsync();

                return Results.Ok(users);
            });

            // Update user
            app.MapPut("/users/{userId}", async (SmartChefDbContext db, int userId, User updatedUser) =>
            {
                var user = await db.Users.FindAsync(userId);

                if (user == null)
                {
                    return Results.NotFound("User not found.");
                }

                // Check for unique email
                if (!string.IsNullOrWhiteSpace(updatedUser.Email) && updatedUser.Email != user.Email)
                {
                    var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == updatedUser.Email);
                    if (existingUser != null)
                    {
                        return Results.BadRequest("A user with this email already exists.");
                    }
                }

                // Update the user's details
                user.Username = updatedUser.Username ?? user.Username;
                user.Email = updatedUser.Email ?? user.Email;

                await db.SaveChangesAsync();

                return Results.Ok(user);
            });

            // Delete user
            app.MapDelete("/users/{userId}", async (SmartChefDbContext db, int userId) =>
            {
                var user = await db.Users.FindAsync(userId);

                if (user == null)
                {
                    return Results.NotFound("User not found.");
                }

                db.Users.Remove(user);
                await db.SaveChangesAsync();

                return Results.Ok("User successfully deleted.");
            });
        }
    }
}


