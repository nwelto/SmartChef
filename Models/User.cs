namespace SmartChef.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirebaseUid { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public ICollection<UserRecipe> UserRecipes { get; set; }
    }

}
