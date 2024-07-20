using Microsoft.EntityFrameworkCore;
using SmartChef;
using SmartChef.Models;
using SmartChef.Dtos;
using Microsoft.Extensions.Logging;
using SmartChef.Dtos.SmartChef.Dtos;

namespace SmartChef.API
{
    public static class UsersAPI
    {
        public static void Map(WebApplication app)
        {
            // Check User
            app.MapPost("/checkUser/{uid}", (SmartChefDbContext db, string uid) =>
            {
                var user = db.Users.Where(u => u.Uid == uid).FirstOrDefault();

                if (user == null)
                {
                    return Results.NotFound("User not registered");
                }

                return Results.Ok(user);
            });

            // Register User
            app.MapPost("/registerUser", (SmartChefDbContext db, UserDto newUserDto) =>
            {
                // Check if the email is already registered
                var existingUser = db.Users.FirstOrDefault(u => u.Email == newUserDto.Email);
                if (existingUser != null)
                {
                    return Results.Conflict("Email already registered");
                }

                // Create a new User entity from the provided DTO
                var newUser = new User
                {
                    UserName = newUserDto.UserName,
                    Email = newUserDto.Email,
                    Uid = newUserDto.Uid
                };

                db.Users.Add(newUser);
                db.SaveChanges();

                return Results.Created($"/users/{newUser.Id}", newUser);
            });

            // Update User
            app.MapPut("/updateUser/{userId}", (SmartChefDbContext db, int userId, UserDto updatedUserDto) =>
            {
                var userToUpdate = db.Users.Find(userId);

                if (userToUpdate == null)
                {
                    return Results.NotFound(); // return a 404 Not Found response
                }

                userToUpdate.UserName = updatedUserDto.UserName;
                userToUpdate.Email = updatedUserDto.Email;

                db.SaveChanges();

                // Return a 200 OK response with the updated user details in the response body
                return Results.Ok(userToUpdate);
            });

            // Get single user's details
            app.MapGet("/singleUser/{userId}", (SmartChefDbContext db, int userId) =>
            {
                var singleUser = db.Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.Uid
                })
                .SingleOrDefault();

                if (singleUser == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(singleUser);
            });
        }
    }
}




