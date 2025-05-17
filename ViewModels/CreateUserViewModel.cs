namespace Dashboard.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class CreateUserViewModel
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Role { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }

}
