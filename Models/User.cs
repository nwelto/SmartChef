namespace SmartChef.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Uid { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }

        public ICollection<UserRecipe> UserRecipes { get; set; }
    }

}
