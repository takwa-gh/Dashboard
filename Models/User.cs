namespace Dashboard.Models
{
    public class User

    {
        public int UserId { get; set; }= new int();
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } // e.g., "admin", "user"

    }
}
