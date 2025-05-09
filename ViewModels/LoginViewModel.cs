namespace Dashboard.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        [Required(ErrorMessage ="Enter your email!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
