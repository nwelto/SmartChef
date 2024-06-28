namespace SmartChef.Models
{
    public class UserRecipe
    {
        public int UserRecipeId { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public User User { get; set; }
        public Recipe Recipe { get; set; }
    }
}
