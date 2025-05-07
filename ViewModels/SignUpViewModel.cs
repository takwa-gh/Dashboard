using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class SignUpViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }
    }

}
