using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class UserViewModel
    {
       
        public int UserId { get; set; }
        [Required]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Adresse email")]
        public required string Email { get; set; }

        [Required]
        [Display(Name = "Rôle")]
        public required string Role { get; set; }
    }

}
