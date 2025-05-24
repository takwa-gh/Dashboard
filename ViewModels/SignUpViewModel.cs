using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class SignUpViewModel
    {
        [Required]

        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public String ConfirmPassword { get; set; } = string.Empty;
    }

}
