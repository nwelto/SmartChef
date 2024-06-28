namespace SmartChef.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public string Ingredients { get; set; }
        public string ImageUrl { get; set; }
        public string SourceUrl { get; set; }
        public ICollection<UserRecipe> UserRecipes { get; set; }
    }
}
