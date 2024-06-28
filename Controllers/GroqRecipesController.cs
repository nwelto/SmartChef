using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartChef.Models;
using GroqApiLibrary;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SmartChef.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly SmartChefDbContext _context;
        private readonly IGroqApiClient _groqApiClient;

        public RecipesController(SmartChefDbContext context, IGroqApiClient groqApiClient)
        {
            _context = context;
            _groqApiClient = groqApiClient;
        }

        [HttpPost("generate-recipe")]
        public async Task<IActionResult> GenerateRecipe([FromBody] string[] ingredients)
        {
            if (ingredients == null || ingredients.Length == 0)
            {
                return BadRequest("The ingredients field is required.");
            }

            string ingredientList = string.Join(", ", ingredients);
            string prompt = $"Create a recipe using only the following ingredients: {ingredientList}. Please provide a title and step-by-step instructions. Do not include any other ingredients.";

            JsonObject request = new JsonObject
            {
                ["model"] = "mixtral-8x7b-32768",
                ["messages"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["role"] = "user",
                        ["content"] = prompt
                    }
                }
            };

            JsonObject? result = await _groqApiClient.CreateChatCompletionAsync(request);

            if (result == null)
            {
                return StatusCode(500, "Failed to get a response from Groq API.");
            }

            string content = result["choices"]?[0]?["message"]?["content"]?.ToString() ?? "No content";
            string[] splitContent = content.Split(new string[] { "\n\nInstructions:\n\n" }, StringSplitOptions.None);

            if (splitContent.Length < 2)
            {
                return StatusCode(500, "Failed to parse the generated recipe.");
            }

            string title = splitContent[0].Replace("Title: ", "").Trim();
            string instructions = splitContent[1].Trim();

            var recipe = new Recipe
            {
                Title = title,
                Instructions = instructions,
                Ingredients = ingredientList
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return Ok(recipe);
        }

        // Get all recipes
        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _context.Recipes.ToListAsync();
            return Ok(recipes);
        }

        // Get a recipe by ID
        [HttpGet("{recipeId}")]
        public async Task<IActionResult> GetRecipeById(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            return Ok(recipe);
        }

        // Update a recipe
        [HttpPut("{recipeId}")]
        public async Task<IActionResult> UpdateRecipe(int recipeId, [FromBody] Recipe updatedRecipe)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            recipe.Title = updatedRecipe.Title ?? recipe.Title;
            recipe.Instructions = updatedRecipe.Instructions ?? recipe.Instructions;
            recipe.Ingredients = updatedRecipe.Ingredients ?? recipe.Ingredients;
            recipe.ImageUrl = updatedRecipe.ImageUrl ?? recipe.ImageUrl;
            recipe.SourceUrl = updatedRecipe.SourceUrl ?? recipe.SourceUrl;

            await _context.SaveChangesAsync();

            return Ok(recipe);
        }

        // Delete a recipe
        [HttpDelete("{recipeId}")]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return Ok("Recipe successfully deleted.");
        }
    }
}





